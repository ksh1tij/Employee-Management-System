/// <reference types="@angular/localize" />

import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app/app.routes';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { provideAnimations } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { provideHttpClient } from '@angular/common/http';
import { environment } from './environments/environment';
import { BASE_URL, TOASTER_CONFIG } from './app/app.config';
import { InitService } from './app/init.service';
import { provideAppInitializer } from '@angular/core';
import { provideToastr } from 'ngx-toastr';

registerLocaleData(en);

function initializeApp(initService: InitService): Promise<void> {
  return initService.init();
}

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(RouterModule, AppRoutingModule, ReactiveFormsModule, FormsModule),
    provideAnimations(),
    provideHttpClient(),
    provideToastr(),
    { provide: BASE_URL, useValue: environment.apiUrl },
    {
      provide: TOASTER_CONFIG,
      useValue: {
        timeOut: 3000,
        positionClass: 'toast-bottom-right',
        preventDuplicates: true,
      }
    },
    provideAppInitializer(() => initializeApp(new InitService(environment.apiUrl)))
  ]
}).catch(err => console.error(err));