import { Observable, tap } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BASE_URL, TOASTER_CONFIG, JWT_TOKEN } from '../../../app.config';
import { ToastrService } from 'ngx-toastr';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    @Inject(BASE_URL) private baseUrl: string,
    @Inject(TOASTER_CONFIG) private toasterConfig: any,
    private toastr: ToastrService
  ) {
    console.log('BASE_URL in AuthService:', this.baseUrl);
    console.log('TOASTER_CONFIG in AuthService:', this.toasterConfig);
  }

  login(credentials: any): Observable<any> {
    console.log('Attempting login with BASE_URL:', this.baseUrl);
    localStorage.clear();
    return this.http.post<any>(`${this.baseUrl}/Authenticate/Login`, credentials).pipe(
      tap(response => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          const decodedToken: any = jwtDecode(response.token);
          const userId = decodedToken.id;
          const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
          const username = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
        
          // Store the extracted information in local storage
          localStorage.setItem('userId', userId);
          localStorage.setItem('userRole', userRole);
          localStorage.setItem('username', username);
        }
      })
    );
  }

  register(user: { username: string; password: string; email: string }): Observable<any> {
    console.log('Attempting registration with BASE_URL:', this.baseUrl);
    return this.http.post<any>(`${this.baseUrl}/Authenticate/Register`, user).pipe(
      tap(response => {
        if (response.success) {
          this.toastr.success('Registration successful', 'Success', this.toasterConfig);
        } else {
          this.toastr.error('Registration failed', 'Error', this.toasterConfig);
        }
      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.clear();
    this.toastr.info('Logged out successfully', 'Logout', this.toasterConfig);
  }

  showSuccess(message: string) {
    this.toastr.success(message, 'Success', this.toasterConfig);
  }
}