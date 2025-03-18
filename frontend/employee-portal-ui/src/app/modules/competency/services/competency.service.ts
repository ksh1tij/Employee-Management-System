import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BASE_URL } from '../../../app.config';
import { Observable } from 'rxjs';
import { Competency } from '../models/competency.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CompetencyService {
  private apiUrl = `${environment.apiUrl}/competency`;

  constructor(private http: HttpClient) {}

  getUserCompetencies(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/User/${userId}`);
  }

  getUsersUnderManager(managerId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/Manager/${managerId}`);
  }

  // getCompetency(id: number): Observable<Competency> {
  //   return this.http.get<Competency>(`${this.apiUrl}/${id}`);
  // }

  getCompetency(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  postCompetency(competencyDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.post(this.apiUrl, competencyDto, { headers });
  }

  deleteCompetency(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  patchCompetency(id: number, competencyDto: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.patch(`${this.apiUrl}/${id}`, competencyDto, { headers });
  }
}
