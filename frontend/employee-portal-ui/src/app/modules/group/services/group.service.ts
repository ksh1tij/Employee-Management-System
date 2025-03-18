import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import * as jsonpatch from 'fast-json-patch';

// Models
export interface UserGroup {
  groupId: number;
  groupName: string;
  members?: User[];
}

export interface User {
  userId: number;
  name: string;
  email: string;
  // Add other user properties as needed
}

export interface UserGroupDto {
  userId: number;
  groupId: number;
  groupName: string;
}

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  private apiUrl = `${environment.apiUrl}/UserGroup`;

  constructor(private http: HttpClient) { }

  getUserGroupById(id: number): Observable<UserGroup> {
    return this.http.get<UserGroup>(`${this.apiUrl}/${id}`);
  }

  getUserGroups(userId: number): Observable<UserGroup[]> {
    return this.http.get<UserGroup[]>(`${this.apiUrl}/User/${userId}`);
  }

  getUsersUnderManager(managerId: number): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Manager/${managerId}`);
  }

  postUserGroup(userGroupDto: UserGroupDto): Observable<UserGroup> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<UserGroup>(this.apiUrl, userGroupDto, { headers });
  }

  deleteUserGroup(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  patchUserGroup(id: number, userGroupDto: UserGroupDto): Observable<void> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.patch<void>(`${this.apiUrl}/${id}`, userGroupDto, { headers });
  }
}
