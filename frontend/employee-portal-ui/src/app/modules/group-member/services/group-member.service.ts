import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

// Models
export interface UserGroupMember {
  userId: number;
  groupId: number;
  user: User;
}

export interface User {
  userId: number;
  userName: string;
  // Add other user properties as needed
}

export interface UserGroupMemberDto {
  userId: number;
  groupId: number;
  groupName?: string; // Optional, depending on your use case
}

@Injectable({
  providedIn: 'root'
})
export class GroupMemberService {
  private apiUrl = `${environment.apiUrl}/UserGroupMember`;

  constructor(private http: HttpClient) { }

  getGroupMembers(groupId: number): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/Group/${groupId}`);
  }

  getUserGroupMembersUnderManager(managerId: number): Observable<UserGroupMember[]> {
    return this.http.get<UserGroupMember[]>(`${this.apiUrl}/Manager/${managerId}`);
  }

  postUserGroupMember(userGroupMemberDto: UserGroupMemberDto): Observable<UserGroupMember> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<UserGroupMember>(this.apiUrl, userGroupMemberDto, { headers });
  }

  deleteUserGroupMember(userId: number, groupId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${userId}/${groupId}`);
  }

  patchUserGroupMember(userId: number, groupId: number, userGroupMemberDto: UserGroupMemberDto): Observable<void> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.patch<void>(`${this.apiUrl}/${userId}/${groupId}`, userGroupMemberDto, { headers });
  }
}
