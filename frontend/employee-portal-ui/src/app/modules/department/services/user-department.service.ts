import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserDepartmentService {
  private apiUrl = `${environment.apiUrl}/UserDepartment`;

  constructor(private http: HttpClient) {}

  getUserDepartments(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/User/${userId}`, { responseType: 'text' }).pipe(
      map(response => JSON.parse(response) as any)
    );
  }
  
  getManagerDepartments(managerId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/Manager/${managerId}`);
  }
  
  createUserDepartment(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }
  
  deleteUserDepartment(userId: number, departmentId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${userId}/${departmentId}`);
  }
  
  updateUserDepartment(userId: number, departmentId: number, data: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${userId}/${departmentId}`, data);
  }
  
  getUsersByDepartmentId(departmentId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${departmentId}/Users`, { responseType: 'text' }).pipe(
      map(response => JSON.parse(response) as any)
    );
  }
  
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      console.error('An error occurred:', error.error.message);
    } else {
      // Backend returned an unsuccessful response code
      console.error(`Backend returned code ${error.status}, body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message
    return throwError('Something bad happened; please try again later.');
  }
}