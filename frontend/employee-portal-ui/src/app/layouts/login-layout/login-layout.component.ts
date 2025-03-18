import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login-layout',
  templateUrl: './login-layout.component.html',
  standalone: true,
  imports: [
    RouterModule, // Import RouterModule
  ],
  styleUrls: ['./login-layout.component.css']
})
export class LoginLayoutComponent {}