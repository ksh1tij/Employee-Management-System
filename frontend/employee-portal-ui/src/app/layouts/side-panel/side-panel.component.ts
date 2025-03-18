//Todo - fix the mobile screen of sidenav below 780px make it so that is collapse and only show icon

import { Component, OnInit, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/authentication/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../../modules/user/services/user.service';
import *as bootstrap from 'bootstrap';
import { FormsModule } from '@angular/forms';
import { saveAs } from 'file-saver'; // Import file-saver
import { PdfService } from '../../core/pdf/services/pdf.service'; // Import the PdfService
import { Tooltip } from 'bootstrap';

interface UserDto {
  userId: number;
  name: string;
  email: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: Date;
  dateOfJoining: Date;
  designation: string;
  role: string;
  userName: string;
  originalPassword: string;
  newPassword: string;
}

@Component({
  selector: 'app-side-panel',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  templateUrl: './side-panel.component.html',
  styleUrls: ['./side-panel.component.css']
})
export class SidePanelComponent {
  constructor(private router: Router,
    private authService: AuthService,
    private http: HttpClient,
    private userService: UserService,
    private pdfService: PdfService // Inject the PdfService
  ) {}

  activeLink: HTMLElement | null = null;
  isCollapsed = false;
  isManager = false; // Add this property
  userName?: string;
  userDto: UserDto = {
    userId: Number(localStorage.getItem("userId")),
    name: '',
    email: '',
    phoneNumber: '',
    address: '',
    dateOfBirth: new Date(),
    dateOfJoining: new Date(),
    designation: '',
    role: '',
    userName: '',
    originalPassword: '',
    newPassword: ''
  };
  originalUserDto: UserDto = { ...this.userDto };

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.isCollapsed = window.innerWidth < 780;
  }

  ngOnInit() {
    this.userName = localStorage.getItem('username') || '';
    this.isCollapsed = window.innerWidth < 780;
    this.checkIfManager(); // Check if the user is a manager
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
      return new Tooltip(tooltipTriggerEl);
    });

  }

  checkIfManager(): void {
    // Assuming the role is stored in localStorage
    const role = localStorage.getItem('userRole');
    this.isManager = role === 'Manager';
  }

  setActive(event: Event) {
    const target = (event.currentTarget as HTMLElement).closest('a');
    if (this.activeLink) {
      this.activeLink.classList.remove('active');
    }
    if (target) {
      target.classList.add('active');
      this.activeLink = target;
    }
  }

  navigate(path: string) {
    this.router.navigate([path]);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  formatDate(date: Date): string {
    return date.toString();
  }

  // updateUser(id: number, user: UserDto): Observable<void> {
  //   return this.http.patch<void>(`${this.apiUrl}/${id}`, user);
  // }

  fetchUserInfo(userId: number): void {
    this.userService.getUser(this.userDto.userId).subscribe({
      next: (user) => {
        this.userDto = {
          ...user,
          dateOfBirth: new Date(user.dateOfBirth),
          dateOfJoining: new Date(user.dateOfJoining),
          originalPassword: '',
          newPassword: ''
        };
        this.originalUserDto = { ...this.userDto };
        console.log(this.originalUserDto);
      },
      error: (err) => console.error('Error fetching user info', err)
    });
  }

  onUpdateUser(): void {
    const updatedUserDto: any = {
      userId: this.userDto.userId,
      name: this.userDto.name || this.originalUserDto.name,
      email: this.userDto.email || this.originalUserDto.email,
      phoneNumber: this.userDto.phoneNumber || this.originalUserDto.phoneNumber,
      address: this.userDto.address || this.originalUserDto.address,
      dateOfBirth: this.userDto.dateOfBirth.toISOString(),
      dateOfJoining: this.userDto.dateOfJoining.toISOString(),
      designation: this.userDto.designation || this.originalUserDto.designation,
      role: this.userDto.role || this.originalUserDto.role,
      userName: this.userDto.userName || this.originalUserDto.userName,
      originalPassword: this.userDto.originalPassword,
      newPassword: this.userDto.newPassword
    };
  
    // const payload = {
    //   userDto: updatedUserDto,
    //   originalPassword: this.userDto.originalPassword,
    //   newPassword: this.userDto.newPassword
    // };

    console.log(updatedUserDto);
  
    this.userService.updateUser(this.userDto.userId, updatedUserDto).subscribe({
      next: () => {
        console.log('User updated successfully');
        console.log(updatedUserDto);
        const userInfoModalElement = document.getElementById('userInfoModal');
        if (userInfoModalElement) {
          const bootstrapModal = bootstrap.Modal.getInstance(userInfoModalElement);
          if (bootstrapModal) {
            bootstrapModal.hide();
          }
        }
        const successModalElement = document.getElementById('successModal');
        if (successModalElement) {
          const successBootstrapModal = new bootstrap.Modal(successModalElement);
          successBootstrapModal.show();
        }
      },
      error: (err) => {
        console.error('Error updating user', err);
        const userInfoModalElement = document.getElementById('userInfoModal');
        if (userInfoModalElement) {
          const bootstrapModal = bootstrap.Modal.getInstance(userInfoModalElement);
          if (bootstrapModal) {
            bootstrapModal.hide();
          }
        }
        const errorModalElement = document.getElementById('errorModal');
        if (errorModalElement) {
          const errorBootstrapModal = new bootstrap.Modal(errorModalElement);
          errorBootstrapModal.show();
        }
      }
    });
  }

  // Method to generate PDF
  generatePdf() {
    const payslipDto = {
      userId: this.userDto.userId,
      // Add other necessary properties here
    };

    this.pdfService.generatePdf(payslipDto).subscribe(
      (response: Blob) => {
        const fileName = 'Payslip.pdf'; // You can dynamically generate the file name if needed
        saveAs(response, fileName);

        // Close userInfoModal
        const userInfoModalElement = document.getElementById('userInfoModal');
        if (userInfoModalElement) {
          const bootstrapModal = bootstrap.Modal.getInstance(userInfoModalElement);
          if (bootstrapModal) {
            bootstrapModal.hide();
          }
        }

        // Show success modal
        const successModalElement = document.getElementById('successModal');
        if (successModalElement) {
          const successBootstrapModal = new bootstrap.Modal(successModalElement);
          successBootstrapModal.show();

          // Automatically close the success modal after 3 seconds
          setTimeout(() => {
            successBootstrapModal.hide();
            document.body.classList.remove('modal-open');
            const backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) {
              backdrop.remove();
            }
          }, 3000);
          const backdrop = document.querySelector('.modal-backdrop');
          if (backdrop) {
            backdrop.remove();
          }
        }
      },
      (error) => {
        console.error('Error generating PDF:', error);
      }
    );
  }
}