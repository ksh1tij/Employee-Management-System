import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { provideCharts } from 'ng2-charts';
import { registerables } from 'chart.js';
import { HttpClientModule } from '@angular/common/http';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

bootstrapApplication(AppComponent, {
  providers: [
    // importProvidersFrom(BrowserModule),
    provideHttpClient(withInterceptorsFromDi()),
    provideCharts({ registerables })
  ]
}).catch(err => console.error(err));