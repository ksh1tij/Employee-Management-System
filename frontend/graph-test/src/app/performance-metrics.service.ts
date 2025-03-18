import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PerformanceMetricsService {
  private apiUrl = 'http://localhost:5246/api/PerformanceMetric/User';

  constructor(private http: HttpClient) { }

  getUserPerformanceMetrics(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${userId}`);
  }
}