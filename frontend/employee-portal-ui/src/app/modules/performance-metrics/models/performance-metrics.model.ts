export interface PerformanceMetric {
    metricId?: number;
    userId: number;
    taskCompletionRate: number;
    qualityOfWork: number;
    attendanceRate: number;
    customerSatisfaction: number;
    efficiency: number;
    teamwork: number;
    lastUpdated: string;
    managerId: number;
}