import { DepartmentService } from '../../services/department.service';
import { CommonModule } from '@angular/common';
import { UserService } from '../../../user/services/user.service';
import { UserDepartmentService } from '../../services/user-department.service';
import { forkJoin } from 'rxjs';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Operation } from 'fast-json-patch';
import { FormsModule } from '@angular/forms';

interface Department {
  departmentId: number;
  departmentName: string;
  managerId: number;
  manager: {
    userId: number;
    name: string;
    email: string;
    designation: string;
  };
  members: {
    userId: number;
    name: string;
    email: string;
    designation: string;
  }[];
  showMembers?: boolean; // Optional property to control visibility
}

interface User {
  userId: number;
  name: string;
  email: string;
  role: string;
  designation: string;
}

interface UserDto {
  userId: number;
  name: string;
  email: string;
}

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  departments: Department[] = [];
  currentUserDepartmentId: number | null = null;
  newDepartmentName: string = '';
  editDepartmentName: string = '';
  editDepartmentId!: number;
  editDepartmentMembers: User[] = [];
  allUsers: User[] = [];
  userRole: string = '';
  modalRef!: NgbModalRef;
  editModalRef!: NgbModalRef;

  @ViewChild('createDepartmentModal') createDepartmentModal!: TemplateRef<any>; // Reference to the modal template
  @ViewChild('editDepartmentModal') editDepartmentModal!: TemplateRef<any>; // Reference to the edit modal template

  constructor(
    private departmentService: DepartmentService,
    private userService: UserService,
    private userDepartmentService: UserDepartmentService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole') || '';
    this.fetchCurrentUserDepartment();
  }

  fetchCurrentUserDepartment(): void {
    const userId = Number(localStorage.getItem('userId')); // Assuming userId is stored in localStorage
    this.userDepartmentService.getUserDepartments(userId).subscribe(
      (userDepartments) => {
        console.log(userDepartments);
        if (userDepartments && userDepartments.length > 0) {
          this.currentUserDepartmentId = userDepartments[0].departmentId;
          console.log(userDepartments[0].departmentId);
          console.log(this.currentUserDepartmentId);
          this.fetchDepartments();
        }
      },
      (error) => {
        console.error('Error fetching user departments', error);
      }
    );
  }

  fetchDepartments(): void {
    if (this.currentUserDepartmentId !== null) {
      this.departmentService.getDepartments().subscribe(
        (data) => {
          this.departments = data
            .filter((department: any) => department.departmentId === this.currentUserDepartmentId)
            .map((department: any) => ({
              ...department,
              showMembers: false, // Initially hide members for all departments
              members: []
            }));
          this.fetchManagers();
        },
        (error) => {
          console.error('Error fetching departments', error);
        }
      );
    }
  }

  fetchManagers(): void {
    this.departments.forEach((department: Department) => {
      this.userService.getUser(department.managerId).subscribe(
        (data: User) => {
          department.manager = {
            userId: data.userId,
            name: data.name,
            email: data.email,
            designation: data.designation
          };
        },
        (error) => {
          console.error('Error fetching manager', error);
        }
      );
    });
  }

  fetchAllUsers(): void {
    this.userService.getUsers().subscribe(
      (users: User[]) => {
        this.allUsers = users.filter(user => user.role !== 'Admin').map(user => ({
          userId: user.userId,
          name: user.name,
          email: user.email,
          role: user.role,
          designation: user.designation
        }));
      },
      (error) => {
        console.error('Error fetching all users', error);
      }
    );
  }

  toggleMembers(departmentId: number): void {
    const department = this.departments.find((dep: Department) => dep.departmentId === departmentId);
    if (department) {
      department.showMembers = !department.showMembers;
      if (department.showMembers) {
        this.fetchDepartmentMembers(departmentId);
      }
    }
  }

  fetchDepartmentMembers(departmentId: number): void {
    this.userDepartmentService.getUsersByDepartmentId(departmentId).subscribe(
      (users: UserDto[]) => {
        const department = this.departments.find(dep => dep.departmentId === departmentId);
        if (department) {
          const userObservables = users.map(member => this.userService.getUser(member.userId));
          forkJoin(userObservables).subscribe(
            (users: User[]) => {
              department.members = users.map(user => ({
                userId: user.userId,
                name: user.name,
                email: user.email,
                designation: user.designation
              }));
            },
            (error) => {
              console.error('Error fetching user details', error);
            }
          );
        }
      },
      (error) => {
        console.error('Error fetching department members', error);
      }
    );
  }

  openCreateDepartmentModal(): void {
    this.modalRef = this.modalService.open(this.createDepartmentModal);
  }

  createDepartment(): void {
    const newDepartment = { departmentName: this.newDepartmentName };
    this.departmentService.createDepartment(newDepartment).subscribe(
      (response) => {
        console.log('Department created successfully', response);
        alert('Department created successfully'); // Show success popup
        this.modalRef.close();
        this.newDepartmentName = ''; // Reset new department form
        this.fetchDepartments(); // Refresh the department list
      },
      (error) => {
        console.error('Error creating department', error);
      }
    );
  }

  openEditDepartmentModal(department: Department): void {
    this.editDepartmentName = department.departmentName;
    this.editDepartmentId = department.departmentId;
    this.editModalRef = this.modalService.open(this.editDepartmentModal);
    this.fetchEditDepartmentMembers(department.departmentId);
    this.fetchAllUsers();
  }

  fetchEditDepartmentMembers(departmentId: number): void {
    this.userDepartmentService.getUsersByDepartmentId(departmentId).subscribe(
      (data: UserDto[]) => {
        const userObservables = data.map(member => this.userService.getUser(member.userId));
        forkJoin(userObservables).subscribe(
          (users: User[]) => {
            this.editDepartmentMembers = users.map(user => ({
              userId: user.userId,
              name: user.name,
              email: user.email,
              designation: user.designation,
              role: user.role // Ensure role is included
            }));
            this.filterUsersNotInDepartment();
          },
          (error) => {
            console.error('Error fetching user details', error);
          }
        );
      },
      (error) => {
        console.error('Error fetching department members', error);
      }
    );
  }

  filterUsersNotInDepartment(): void {
    const memberIds = this.editDepartmentMembers.map(member => member.userId);
    this.allUsers = this.allUsers.filter(user => !memberIds.includes(user.userId));
  }

  editDepartment(departmentId: number): void {
    const updatedDepartment = {
      departmentId: departmentId,
      departmentName: this.editDepartmentName,
      managerId: this.departments.find(dep => dep.departmentId === departmentId)?.managerId || 0
    };
    this.departmentService.updateDepartment(departmentId, updatedDepartment).subscribe(
      () => {
        console.log('Department updated successfully');
        alert('Department updated successfully'); // Show success popup
        this.editModalRef.close();
        this.fetchDepartments(); // Refresh the department list
      },
      (error) => {
        console.error('Error updating department', error);
      }
    );
  }

  removeUserFromDepartment(userId: number, event: Event): void {
    event.stopPropagation(); // Prevent form submission
    if (confirm('Are you sure you want to remove this user from the department?')) {
      this.userDepartmentService.deleteUserDepartment(userId, this.editDepartmentId).subscribe(
        () => {
          console.log('User removed from department successfully');
          alert('User removed from department successfully'); // Show success popup
          this.fetchEditDepartmentMembers(this.editDepartmentId); // Refresh the department members list in the modal
        },
        (error) => {
          console.error('Error removing user from department', error);
        }
      );
    }
  }

  addUserToDepartment(userId: number): void {
    const userDepartmentDto = { userId, departmentId: this.editDepartmentId };
    this.userDepartmentService.createUserDepartment(userDepartmentDto).subscribe(
      () => {
        console.log('User added to department successfully');
        alert('User added to department successfully'); // Show success popup
        this.fetchEditDepartmentMembers(this.editDepartmentId); // Refresh the department members list in the modal
      },
      (error) => {
        console.error('Error adding user to department', error);
      }
    );
  }

  confirmDeleteDepartment(departmentId: number): void {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.deleteDepartment(departmentId).subscribe(
        () => {
          console.log('Department deleted successfully');
          alert('Department deleted successfully'); // Show success popup
          this.fetchDepartments(); // Refresh the department list
        },
        (error) => {
          console.error('Error deleting department', error);
        }
      );
    }
  }
}