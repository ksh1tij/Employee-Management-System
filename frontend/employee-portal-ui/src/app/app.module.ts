import { NgModule, Inject } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './core/authentication/components/login/login.component';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
// import { AuthInterceptor } from './core/interceptors/http.interceptor';
import { appConfig, BASE_URL, TOASTER_CONFIG, JWT_TOKEN } from './app.config';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { environment } from '../environments/environment';
import { ToastrModule } from 'ngx-toastr';


export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    LoginComponent,
    MatIconModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
    ToastrModule.forRoot(),
    MatCardModule,
    AppComponent // Import AppComponent here
  ],
  providers: [
    { provide: BASE_URL, useValue: environment.apiUrl },
    {
      provide: TOASTER_CONFIG,
      useValue: {
        timeOut: 3000,
        positionClass: 'toast-top-right',
        preventDuplicates: true,
      }
    },
    // { provide: JWT_TOKEN, useValue: tokenGetter }, // Replace with actual token logic
    // { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ]
})
export class AppModule { 
  constructor(@Inject(BASE_URL) private baseUrl: string) {
    console.log('BASE_URL in AppModule:', this.baseUrl);
  }
}