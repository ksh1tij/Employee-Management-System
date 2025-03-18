import { ApplicationConfig, importProvidersFrom, Provider, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { InjectionToken } from '@angular/core';
import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { ToastrModule } from 'ngx-toastr';
import { provideHttpClient, HTTP_INTERCEPTORS, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './core/interceptors/http.interceptor'; // Adjust the path as necessary


export const BASE_URL = new InjectionToken<string>('BASE_URL');
export const TOASTER_CONFIG = new InjectionToken<any>('TOASTER_CONFIG');
export const JWT_TOKEN = new InjectionToken<string>('JWT_TOKEN');

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideZoneChangeDetection({eventCoalescing: true}),
    { provide: BASE_URL, useValue: environment.apiUrl },
    {
      provide: TOASTER_CONFIG,
      useValue: {
        timeOut: 3000,
        positionClass: 'toast-top-right',
        preventDuplicates: true,
      }
    },
    // { provide: JWT_TOKEN, useValue: 'your-jwt-token' }, // Replace with actual token logic
    importProvidersFrom(ToastrModule.forRoot()),
    // { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true } // Add this line
  ]
};