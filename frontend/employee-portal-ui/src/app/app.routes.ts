import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LoginComponent } from './core/authentication/components/login/login.component';
import { LoginLayoutComponent } from './layouts/login-layout/login-layout.component';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { DashboardComponent } from './modules/dashboard/components/dashboard/dashboard.component';
import { DepartmentsComponent } from './modules/department/components/departments/departments.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { SettingsComponent } from './settings/settings.component';
import { RegisterComponent } from './core/authentication/components/register/register.component'
import { HomePageComponent } from './modules/home/components/home-page/home-page.component';
import { UserGroupComponent } from './modules/group/components/user-group/user-group.component';
import { PerformanceMetricsComponent } from './modules/performance-metrics/components/performance-metrics/performance-metrics.component';
import { CompetencyComponent } from './modules/competency/components/competency/competency.component';

import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { ManagedUsersComponent } from './modules/dashboard/components/managed-users/managed-users.component';
import { AuthGuard } from './core/guard/auth.guard';


export const routes: Routes = [
  {
    path: '',
    component: LoginLayoutComponent,
    children: [
      { path: '', redirectTo: '/login', pathMatch: 'full'},
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'home', component: HomePageComponent }
    ]
  },
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
      { path: 'department', component: DepartmentsComponent, canActivate: [AuthGuard] },
      { path: 'user-group', component: UserGroupComponent, canActivate: [AuthGuard] },
      { path: 'notifications', component: NotificationsComponent, canActivate: [AuthGuard] },
      { path: 'performance-metrics', component: PerformanceMetricsComponent, canActivate: [AuthGuard] },
      { path: 'competency', component: CompetencyComponent, canActivate: [AuthGuard] },
      { path: 'managed-users', component: ManagedUsersComponent, canActivate: [AuthGuard] },
      // { path: 'settings', component: SettingsComponent },
      // other routes for main layout
    ]
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { enableTracing: false }), 
    ReactiveFormsModule, 
    FormsModule, 
    HttpClientModule,
    BrowserModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    LoginComponent,
    MatIconModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatButtonModule,
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }