import { CommonModule } from '@angular/common';
import { NgxChartsModule, Color, ScaleType } from '@swimlane/ngx-charts';
import { Competency } from '../../../competency/models/competency.model';
import { CompetencyService } from '../../../competency/services/competency.service';
import { DepartmentService } from '../../../department/services/department.service';
import { UserService, UserDto } from '../../../user/services/user.service';
import { PerformanceMetric } from '../../../performance-metrics/models/performance-metrics.model';
import { PerformanceMetricsService } from '../../../performance-metrics/services/performance-metrics.service';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { Chart, registerables } from 'chart.js';
import { Observable } from 'rxjs';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CommonModule,
    NgxChartsModule,
  ],  // Import CommonModule here
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {

  competency: any;
  department: any;

  users: UserDto[] = [];
  selectedUser: any;

  performanceMetrics: any;

  constructor(
    private competencyService: CompetencyService, 
    private departmentService: DepartmentService, 
    private userService: UserService, 
    private performanceMetricService: PerformanceMetricsService
  ) {}

  ngOnInit(): void {
    this.fetchCompetency();
    this.fetchPerformanceMetrics();
    this.fetchDepartment();
    // this.fetchUser();
  }

  // public barChartOptions: ChartOptions = {
  //   responsive: true,
  //   maintainAspectRatio: false,
  //   animation: { duration: 0 },
  // };
  // public barChartData: ChartConfiguration<'bar'>['data'] = {
  //   labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Communication', 'Leadership', 'Problem Solving', 'Teamwork', 'Technical Skills'],
  //   datasets: [{ data: [65, 59, 80, 81, 56, 0, 0, 0, 0, 0], label: 'Expenses & Competency' }],
  // };
 
  // public lineChartOptions: ChartOptions = {
  //   responsive: true,
  //   maintainAspectRatio: false,
  //   animation: { duration: 0 },
  // };
  // public lineChartData: ChartConfiguration<'line'>['data'] = {
  //   labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Attendance Rate', 'Customer Satisfaction', 'Efficiency', 'Quality Of Work', 'Task Completion Rate', 'Teamwork'],
  //   datasets: [{ data: [28, 48, 40, 19, 86, 0, 0, 0, 0, 0, 0], label: 'Income & Performance Metrics', fill: true }],
  // };

  chartData1 = [
    { name: 'Communication', value: 0},
    { name: 'Leadership', value: 0 },
    { name: 'Problem Solving', value: 0 },
    { name: 'Teamwork', value: 0 },
    { name: 'Technical Skills', value: 0 }
  ];

  colorScheme1: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  displayedColumns1: string[] = [ 'communication', 'leadership', 'problemSolving', 'teamwork', 'technicalSkills' ];

  chartData2 = [
    { name: 'Task Completion Rate', value: 0 },
    { name: 'Quality Of Work', value: 0 },
    { name: 'Attendance Rate', value: 0 },
    { name: 'Customer Satisfaction', value: 0 },
    { name: 'Efficiency', value: 0 },
    { name: 'Teamwork', value: 0 }
  ];

  colorScheme2: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  displayedColumns2: string[] = ['attendanceRate', 'customerSatisfaction', 'efficiency', 'qualityOfWork', 'taskCompletionRate', 'teamwork'];


  fetchDepartment(): void {
    const userId = Number(localStorage.getItem('userId'))
    this.departmentService.getDepartmentById(userId).subscribe(
      (data) => {
        this.department = data;
        console.log('Department:', this.department);
        this.fetchUser();
      },
      (error) => {
        console.error('Error fetching department', error);
      }
    );
  }

  fetchUser(): void {
    console
    this.userService.getUser(this.department.managerId).subscribe(
      (data) => {
        this.selectedUser = data;
        console.log('Manager:', this.selectedUser);
      },
      (error) => {
        console.error('Error fetching user', error);
      }
    );
  }

  fetchCompetency(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.competencyService.getCompetency(userId).subscribe(
      (data) => {
        this.competency = data;
        console.log('Competency:', this.competency);
        this.updateChartData1();
      },
      (error) => {
        console.error('Error fetching competency', error);
      }
    );
  }

  fetchPerformanceMetrics(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.performanceMetricService.getUserPerformanceMetrics(userId).subscribe(
      (data) => {
        this.performanceMetrics = data;
        console.log('PM:', this.performanceMetrics);
        this.updateChartData2();
      },
      (error) => {
        console.error('Error fetching competency', error);
      }
    );
  }


  updateChartData1(): void {
    this.chartData1 = [
      { name: 'Communication', value: this.competency.communication },
      { name: 'Leadership', value: this.competency.leadership },
      { name: 'Problem Solving', value: this.competency.problemSolving },
      { name: 'Teamwork', value: this.competency.teamwork },
      { name: 'Technical Skills', value: this.competency.technicalSkills }
    ];
  }

  updateChartData2(): void {
    if (this.performanceMetrics.length > 0) {
      const pm = this.performanceMetrics[0];
      this.chartData2 = [
        { name: 'Task Completion Rate', value: pm.taskCompletionRate },
        { name: 'Quality Of Work', value: pm.qualityOfWork },
        { name: 'Attendance Rate', value: pm.attendanceRate },
        { name: 'Customer Satisfaction', value: pm.customerSatisfaction },
        { name: 'Efficiency', value: pm.efficiency },
        { name: 'Teamwork', value: pm.teamwork }
      ];
    }
  }

  // departmentInfo = {
  //   title: 'Department Info',
  //   content: 'Details about the department...'
  // };

  // replaceDepartmentInfo(): void {
  //   if (this.competency) {
  //     this.departmentInfo = {
  //       title: 'Competency Details',
  //       content: `
  //         Technical Skills: ${this.competency.technicalSkills}, 
  //         Communication: ${this.competency.communication}, 
  //         Problem Solving: ${this.competency.problemSolving}, 
  //         Teamwork: ${this.competency.teamwork}, 
  //         Leadership: ${this.competency.leadership}, 
  //         Last Updated: ${this.competency.lastUpdated}
  //       `
  //     };
  //   }
  //   console.log(this.departmentInfo.content);
  // }
}