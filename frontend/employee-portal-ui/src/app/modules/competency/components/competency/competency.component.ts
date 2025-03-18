import { CommonModule } from '@angular/common';
import { NgxChartsModule, Color, ScaleType } from '@swimlane/ngx-charts';
import { MatTableModule } from '@angular/material/table';
import { CompetencyService } from '../../services/competency.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UserDepartmentService } from '../../../department/services/user-department.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../../user/services/user.service';

interface Competency {
  userId: number;
  technicalSkills: number;
  communication: number;
  problemSolving: number;
  teamwork: number;
  leadership: number;
  lastUpdated: string;
  managerId: number;
}

@Component({
  selector: 'app-competency',
  imports: [
    MatTableModule,
    CommonModule,
    NgxChartsModule,
    FormsModule
  ],
  templateUrl: './competency.component.html',
  styleUrl: './competency.component.css'
})
export class CompetencyComponent {

  @ViewChild('userInfoModal') userInfoModal!: TemplateRef<any>; // Reference to the modal template
  competencies: Competency[] = [];
  usersUnderManager: any[] = []; // Use the User interface
  userNames: { [key: number]: string } = {}; // Dictionary to store user names
  newCompetency: Competency = {
    userId: 0,
    technicalSkills: 0,
    communication: 0,
    problemSolving: 0,
    teamwork: 0,
    leadership: 0,
    lastUpdated: new Date().toISOString(),
    managerId: 0
  };
  modalRef!: NgbModalRef;

  chartData1 = [
    { name: 'Communication', value: 0},
    { name: 'Leadership', value: 0 },
    { name: 'Problem Solving', value: 0 },
    { name: 'Teamwork', value: 0 },
    { name: 'Technical Skills', value: 0 }
  ];

  colorScheme: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  displayedColumns: string[] = [ 'communication', 'leadership', 'problemSolving', 'teamwork', 'technicalSkills' ];

  constructor(
    private competencyService: CompetencyService,
    private modalService: NgbModal,
    private userService: UserService,
    private userDepartmentService: UserDepartmentService
  ) { }

  ngOnInit(): void {
    this.fetchCompetency();
    this.getUsersUnderManager();
  }

  fetchCompetency(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.competencyService.getCompetency(userId).subscribe(
      (data) => {
        this.competencies = [data];
        console.log("Competency");
        console.log(this.competencies);
        if (this.competencies.length > 0) {
          const competency = this.competencies[0];
          this.chartData1 = [
            { name: 'Communication', value: competency.communication },
            { name: 'Leadership', value: competency.leadership },
            { name: 'Problem Solving', value: competency.problemSolving },
            { name: 'Teamwork', value: competency.teamwork },
            { name: 'Technical Skills', value: competency.technicalSkills }
          ];
        }
      },
      (error) => {
        console.error('Error fetching competency', error);
      }
    );
  }

  getUsersUnderManager(): void {
    const managerId = Number(localStorage.getItem('userId'));
    this.userDepartmentService.getManagerDepartments(managerId).subscribe(
      (data) => {
        this.usersUnderManager = data;
        this.fetchUserNames();
        this.fetchCompetenciesForUsers();
      },
      (error) => {
        console.error('Error fetching users under manager', error);
      }
    );
  }

  fetchCompetenciesForUsers(): void {
    this.usersUnderManager.forEach(user => {
      this.competencyService.getUserCompetencies(user.userId).subscribe(
        (competencies) => {
          const userCompetency = competencies.find((comp: { userId: any; }) => comp.userId === user.userId);
          if (userCompetency) {
            user.competency = userCompetency; // Store competencies under competency key
            console.log(user.competency);
          }
        },
        (error) => {
          console.error('Error fetching competencies for user', error);
        }
      );
    });
    console.log("Competencies for users:");
    console.log(this.usersUnderManager);
  }

  fetchUserNames(): void {
    this.usersUnderManager.forEach(user => {
      this.userService.getUser(user.userId).subscribe(
        (user) => {
          this.userNames[user.userId] = user.name;
        },
        (error) => {
          console.error('Error fetching user details', error);
        }
      );
    });
  }

  openModal(userId: number): void {
    this.newCompetency.userId = userId;
    this.newCompetency.managerId = Number(localStorage.getItem('userId')); // Set the appropriate managerId
    this.modalRef = this.modalService.open(this.userInfoModal);
  }

  postCompetency(): void {
    // Ensure values are between 0 and 100
    this.newCompetency.technicalSkills = Math.max(0, Math.min(100, this.newCompetency.technicalSkills));
    this.newCompetency.communication = Math.max(0, Math.min(100, this.newCompetency.communication));
    this.newCompetency.problemSolving = Math.max(0, Math.min(100, this.newCompetency.problemSolving));
    this.newCompetency.teamwork = Math.max(0, Math.min(100, this.newCompetency.teamwork));
    this.newCompetency.leadership = Math.max(0, Math.min(100, this.newCompetency.leadership));

    this.competencyService.postCompetency(this.newCompetency).subscribe(
      (data: Competency) => {
        console.log('Competency created successfully', data);
        this.modalRef.close();
        this.getUsersUnderManager(); // Refresh users under manager
        this.fetchCompetency(); // Refresh competencies
      },
      (error) => {
        console.error('Error creating competency', error);
      }
    );
  }

  hasCompetency(userId: number): boolean {
    return this.competencies.some(comp => comp.userId === userId);
  }
}
