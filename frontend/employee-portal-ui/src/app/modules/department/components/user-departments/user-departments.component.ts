import { Component, OnInit } from '@angular/core';
import { UserDepartmentService } from '../../services/user-department.service';

@Component({
  selector: 'app-user-departments',
  templateUrl: './user-departments.component.html',
  styleUrls: ['./user-departments.component.css']
})
export class UserDepartmentsComponent implements OnInit {
  userDepartments: any[] = [];

  constructor(private userDepartmentService: UserDepartmentService) {}

  ngOnInit(): void {
    const userId = 1; // Example userId
    this.userDepartmentService.getUserDepartments(userId).subscribe(data => {
      this.userDepartments = data;
    });
  }
}