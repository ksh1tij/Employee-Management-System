import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { appConfig, BASE_URL, TOASTER_CONFIG, JWT_TOKEN } from '../../../app.config';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  constructor(
    @Inject(BASE_URL) private baseUrl: string,
    private http: HttpClient
  ) {
    console.log('BASE_URL in DepartmentService:', this.baseUrl);
  }

  private apiUrl = `${this.baseUrl}/Department`;
  private userDepartmentUrl = `${this.baseUrl}/UserDepartment`;

  getDepartments(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  getDepartmentById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  createDepartment(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  updateDepartment(id: number, data: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}`, data);
  }

  deleteDepartment(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}