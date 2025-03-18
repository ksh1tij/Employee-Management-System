import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { NgxChartsModule, Color, ScaleType } from '@swimlane/ngx-charts';
import { PerformanceMetric } from '../../models/performance-metrics.model';
import { PerformanceMetricsService } from '../../services/performance-metrics.service';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { UserService } from '../../../user/services/user.service';
import { UserDepartmentService } from '../../../department/services/user-department.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

interface User {
  userId: number;
  userName: string;
  name: string;
  email: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: string;
  dateOfJoining: string;
  designation: string;
  managerId: number;
  performanceMetrics?: PerformanceMetric; // Optional property to store performance metrics
}

@Component({
  selector: 'app-performance-metrics',
  imports: [
    MatTableModule,
    CommonModule,
    NgxChartsModule,
    FormsModule
  ],
  templateUrl: './performance-metrics.component.html',
  styleUrl: './performance-metrics.component.css'
})
export class PerformanceMetricsComponent {
  @ViewChild('userInfoModal') userInfoModal!: TemplateRef<any>; // Reference to the modal template
  performanceMetrics: PerformanceMetric[] = [];
  usersUnderManager: User[] = []; // Use the User interface
  userNames: { [key: number]: string } = {}; // Dictionary to store user names
  newPerformanceMetric: PerformanceMetric = {
    userId: 0,
    taskCompletionRate: 0,
    qualityOfWork: 0,
    attendanceRate: 0,
    customerSatisfaction: 0,
    efficiency: 0,
    teamwork: 0,
    lastUpdated: new Date().toISOString(),
    managerId: 0
  };
  modalRef!: NgbModalRef;
  
  chartData1 = [
    { name: 'Task Completion Rate', value: 0 },
    { name: 'Quality Of Work', value: 0 },
    { name: 'Attendance Rate', value: 0 },
    { name: 'Customer Satisfaction', value: 0 },
    { name: 'Efficiency', value: 0 },
    { name: 'Teamwork', value: 0 }
  ];

  colorScheme: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  displayedColumns: string[] = ['attendanceRate', 'customerSatisfaction', 'efficiency', 'qualityOfWork', 'taskCompletionRate', 'teamwork'];

  constructor(
    private performanceMetricService: PerformanceMetricsService, 
    private modalService: NgbModal, 
    private userService: UserService,
    private userDepartmentService: UserDepartmentService
  ) { }

  ngOnInit(): void {
    this.fetchPerformanceMetrics();
    this.getUsersUnderManager();
  }

  fetchPerformanceMetrics(): void {
    const userId = Number(localStorage.getItem('userId'));
    this.performanceMetricService.getUserPerformanceMetrics(userId).subscribe(
      (data) => {
        this.performanceMetrics = data;
        console.log('PM:', this.performanceMetrics);
        if (this.performanceMetrics.length > 0) {
          const pm = this.performanceMetrics[0];
          this.chartData1 = [
            { name: 'Attendance Rate', value: pm.attendanceRate },
            { name: 'Customer Satisfaction', value: pm.customerSatisfaction },
            { name: 'Efficiency', value: pm.efficiency },
            { name: 'Quality Of Work', value: pm.qualityOfWork },
            { name: 'Task Completion Rate', value: pm.taskCompletionRate },
            { name: 'Teamwork', value: pm.teamwork }
          ];
        }
      },
      (error) => {
        console.error('Error fetching performance metrics', error);
      }
    );
  }

  getUsersUnderManager(): void {
    const managerId = Number(localStorage.getItem('userId'));
    this.userDepartmentService.getManagerDepartments(managerId).subscribe(
      (data) => {
        this.usersUnderManager = data;
        this.fetchUserNames();
        this.fetchPerformanceMetricsForUsers();
      },
      (error) => {
        console.error('Error fetching users under manager', error);
      }
    );
  }
  
  fetchPerformanceMetricsForUsers(): void {
    this.usersUnderManager.forEach(user => {
      this.performanceMetricService.getUserPerformanceMetrics(user.userId).subscribe(
        (metrics) => {
          const userMetrics = metrics.find(metric => metric.userId === user.userId);
          if (userMetrics) {
            user.performanceMetrics = userMetrics; // Store metrics under performanceMetrics key
            console.log(user.performanceMetrics);
          }
        },
        (error) => {
          console.error('Error fetching performance metrics for user', error);
        }
      );
    });
    console.log("performance metrics");
    console.log(this.usersUnderManager);
  }

  fetchUserNames(): void {
    this.usersUnderManager.forEach(metric => {
      this.userService.getUser(metric.userId).subscribe(
        (user) => {
          this.userNames[metric.userId] = user.name;
        },
        (error) => {
          console.error('Error fetching user details', error);
        }
      );
    });
  }

  openModal(userId: number): void {
    this.newPerformanceMetric.userId = userId;
    this.newPerformanceMetric.managerId = Number(localStorage.getItem('userId')); // Set the appropriate managerId
    this.modalRef = this.modalService.open(this.userInfoModal);
  }

  postPerformanceMetric(): void {
    // Ensure values are between 0 and 100
    this.newPerformanceMetric.taskCompletionRate = Math.max(0, Math.min(100, this.newPerformanceMetric.taskCompletionRate));
    this.newPerformanceMetric.qualityOfWork = Math.max(0, Math.min(100, this.newPerformanceMetric.qualityOfWork));
    this.newPerformanceMetric.attendanceRate = Math.max(0, Math.min(100, this.newPerformanceMetric.attendanceRate));
    this.newPerformanceMetric.customerSatisfaction = Math.max(0, Math.min(100, this.newPerformanceMetric.customerSatisfaction));
    this.newPerformanceMetric.efficiency = Math.max(0, Math.min(100, this.newPerformanceMetric.efficiency));
    this.newPerformanceMetric.teamwork = Math.max(0, Math.min(100, this.newPerformanceMetric.teamwork));
  
    this.performanceMetricService.postPerformanceMetric(this.newPerformanceMetric).subscribe(
      (data: PerformanceMetric) => {
        console.log('Performance metric created successfully', data);
        this.modalRef.close();
        this.getUsersUnderManager(); // Refresh users under manager
        this.fetchPerformanceMetrics(); // Refresh performance metrics
      },
      (error) => {
        console.error('Error creating performance metric', error);
      }
    );
  }

  hasPerformanceMetrics(userId: number): boolean {
    return this.performanceMetrics.some(metric => metric.userId === userId);
  }
}
