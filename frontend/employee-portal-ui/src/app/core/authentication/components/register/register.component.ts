import { Component } from '@angular/core';
import { NgModule, Inject } from '@angular/core';
import { appConfig, BASE_URL, TOASTER_CONFIG, JWT_TOKEN } from '../../../../app.config';
import { AuthService } from '../../services/auth.service';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  user = {
    username: '',
    password: '',
    email: ''
  };

  // constructor(@Inject(BASE_URL) private baseUrl: string) {
  //   console.log('BASE_URL in AppModule:', this.baseUrl);
  // }

  constructor(
    @Inject(BASE_URL) private baseUrl: string,
    private authService: AuthService,
    private router: Router,
  ) {
    console.log('BASE_URL in RegisterComponent:', this.baseUrl);
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.authService.register(this.user).subscribe(
        response => {
          console.log('Registration successful:', response);
          this.router.navigate(['/dashboard']);
        },
        error => {
          console.error('Registration failed:', error);
        }
      );
    } else {
      console.error('Form is invalid');
    }
  }
}
