import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PerformanceMetric } from '../models/performance-metrics.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class PerformanceMetricsService {

  private apiUrl = `${environment.apiUrl}/PerformanceMetric`;

  constructor(private http: HttpClient) { }

  getUserPerformanceMetrics(userId: number): Observable<PerformanceMetric[]> {
    return this.http.get<PerformanceMetric[]>(`${this.apiUrl}/User/${userId}`);
  }

  getPerformanceMetric(id: number): Observable<PerformanceMetric> {
    return this.http.get<PerformanceMetric>(`${this.apiUrl}/${id}`);
  }

  getUsersUnderManager(managerId: number): Observable<PerformanceMetric[]> {
    return this.http.get<PerformanceMetric[]>(`${this.apiUrl}/Manager/${managerId}`);
  }

  postPerformanceMetric(performanceMetric: PerformanceMetric): Observable<PerformanceMetric> {
    return this.http.post<PerformanceMetric>(this.apiUrl, performanceMetric, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  deletePerformanceMetric(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  patchPerformanceMetric(id: number, performanceMetric: PerformanceMetric): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}`, performanceMetric, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }
}
