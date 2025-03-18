import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompetenciesService {
  private apiUrl = 'http://localhost:5246/api/Competency/User';

  constructor(private http: HttpClient) { }

  getUserCompetencies(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${userId}`);
  }
}