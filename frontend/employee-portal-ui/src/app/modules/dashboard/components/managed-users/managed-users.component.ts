import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { saveAs } from 'file-saver'; // Import file-saver
import * as bootstrap from 'bootstrap';
import { environment } from '../../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { UserDepartmentService } from '../../../department/services/user-department.service';
import { UserService } from '../../../user/services/user.service'; // Import UserService
import { FormsModule } from '@angular/forms';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';


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
  selector: 'app-managed-users',
  imports: [CommonModule, FormsModule],
  templateUrl: './managed-users.component.html',
  styleUrl: './managed-users.component.css'
})
export class ManagedUsersComponent {
  users: any[] = [];
  selectedUser: UserDto = {
    userId: 0,
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
  newUser: UserDto = {
    userId: 0,
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
  apiUrl = environment.apiUrl;
  selectedFile: File | null = null;
  modalRef!: NgbModalRef;

  @ViewChild('createUserModal') createUserModal!: TemplateRef<any>; // Reference to the modal template

  constructor(
    private http: HttpClient,
    private userDepartmentService: UserDepartmentService,
    private userService: UserService, // Inject UserService
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    const managerId = Number(localStorage.getItem('userId'));
    if (managerId) {
      this.userDepartmentService.getManagerDepartments(managerId).subscribe((data: any) => {
        this.users = data;
      });
    } else {
      console.error('Manager ID not found in local storage');
    }

    // Add event listener to remove backdrop when modal is closed
    const modalElement = document.getElementById('userDetailsModal');
    if (modalElement) {
      modalElement.addEventListener('hidden.bs.modal', () => {
        const backdrop = document.querySelector('.modal-backdrop');
        if (backdrop) {
          backdrop.remove();
          backdrop.remove();
        }
      });
    }
  }

  openModal(user: any): void {
    this.selectedUser = { ...user, originalPassword: '', newPassword: '' };
    const modalElement = document.getElementById('userDetailsModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    } else {
      console.error('Modal element not found');
    }
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  uploadFile(): void {
    if (this.selectedFile) {
      this.userService.uploadExcel(this.selectedFile).subscribe(
        (response) => {
          console.log('File uploaded successfully', response);
          alert('File uploaded successfully'); // Show success popup
          this.selectedFile = null; // Clear the selected file
          (document.getElementById('fileInput') as HTMLInputElement).value = ''; // Clear the input field
        },
        (error) => {
          if (error.status === 200 && error.error.text) {
            console.log('File uploaded successfully', error.error.text);
            alert('File uploaded successfully'); // Show success popup
            this.selectedFile = null; // Clear the selected file
            (document.getElementById('fileInput') as HTMLInputElement).value = ''; // Clear the input field
          } else {
            console.error('Error uploading file', error);
          }
        }
      );
    } else {
      console.error('No file selected');
    }
  }
  
  openCreateUserModal(): void {
    this.modalRef = this.modalService.open(this.createUserModal);
  }

  createUser(): void {
    this.userService.createUser(this.newUser).subscribe(
      (response) => {
        console.log('User created successfully', response);
        alert('User created successfully'); // Show success popup
        this.modalRef.close();
        this.newUser = {
          userId: 0,
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
        }; // Reset new user form
        this.ngOnInit(); // Refresh the user list
      },
      (error) => {
        console.error('Error creating user', error);
      }
    );
  }
}
