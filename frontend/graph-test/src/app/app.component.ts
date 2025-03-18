import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { PerformanceMetricsService } from './performance-metrics.service';
import { CompetenciesService } from './competencies.service';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: true,
  imports: [BaseChartDirective, CommonModule]
})
export class AppComponent implements OnInit {
  isBrowser: boolean;
  performanceMetricsData: any;
  competencyData: any;
  chartOptions = {
    responsive: true,
    maintainAspectRatio: true,
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private performanceMetricsService: PerformanceMetricsService,
    private competenciesService: CompetenciesService
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit() {
    const userId = 2; // Replace with dynamic user ID as needed

    this.performanceMetricsService.getUserPerformanceMetrics(userId).subscribe(data => {
      this.performanceMetricsData = {
        labels: ['Task Completion Rate', 'Quality of Work', 'Attendance Rate', 'Customer Satisfaction', 'Efficiency', 'Teamwork'],
        datasets: [
          {
            label: 'Performance Metrics',
            data: [
              data[0].taskCompletionRate,
              data[0].qualityOfWork,
              data[0].attendanceRate,
              data[0].customerSatisfaction,
              data[0].efficiency,
              data[0].teamwork
            ],
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1
          }
        ]
      };
    });

    this.competenciesService.getUserCompetencies(userId).subscribe(data => {
      this.competencyData = {
        labels: ['Technical Skills', 'Communication', 'Problem Solving', 'Teamwork', 'Leadership'],
        datasets: [
          {
            label: 'Competency',
            data: [
              data[0].technicalSkills,
              data[0].communication,
              data[0].problemSolving,
              data[0].teamwork,
              data[0].leadership
            ],
            backgroundColor: 'rgba(153, 102, 255, 0.2)',
            borderColor: 'rgba(153, 102, 255, 1)',
            borderWidth: 1
          }
        ]
      };
    });
  }
}