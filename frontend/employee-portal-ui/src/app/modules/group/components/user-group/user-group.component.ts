import { CommonModule } from '@angular/common';
import { GroupService, User } from '../../services/group.service';
import { GroupMemberService } from '../../../group-member/services/group-member.service';
import { UserService } from '../../../user/services/user.service';
import { forkJoin } from 'rxjs';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

interface UserGroup {
  groupId: number;
  groupName: string;
  members?: User[];
  showDetails?: boolean; // Optional property to control visibility
}

@Component({
  selector: 'app-user-group',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-group.component.html',
  styleUrl: './user-group.component.css'
})
export class UserGroupComponent {
  userGroups: UserGroup[] = [];
  newGroupName: string = '';
  editGroupName: string = '';
  editGroupId!: number;
  editGroupMembers: User[] = [];
  allUsers: User[] = [];
  userRole: string = '';
  modalRef!: NgbModalRef;
  editModalRef!: NgbModalRef;

  @ViewChild('createGroupModal') createGroupModal!: TemplateRef<any>; // Reference to the modal template
  @ViewChild('editGroupModal') editGroupModal!: TemplateRef<any>; // Reference to the edit modal template

  constructor(
    private groupService: GroupService,
    private groupMemberService: GroupMemberService,
    private userService: UserService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.userRole = localStorage.getItem('userRole') || '';
    this.fetchUserGroups();
  }

  fetchUserGroups(): void {
    const userId = Number(localStorage.getItem('userId')); // Assuming userId is stored in localStorage
    this.groupService.getUserGroups(userId).subscribe(
      (data) => {
        this.userGroups = data.map(group => ({ ...group, showDetails: false }));
      },
      (error) => {
        console.error('Error fetching user groups', error);
      }
    );
  }

  fetchAllUsers(): void {
    this.userService.getUsers().subscribe(
      (users) => {
        this.allUsers = users.filter(user => user.role !== 'Admin'); // Filter out admins
      },
      (error) => {
        console.error('Error fetching all users', error);
      }
    );
  }

  toggleDetails(groupId: number): void {
    const group = this.userGroups.find(g => g.groupId === groupId);
    if (group) {
      group.showDetails = !group.showDetails;
      if (group.showDetails ) {
        this.fetchGroupMembers(groupId);
      }
    }
  }

  fetchGroupMembers(groupId: number): void {
    console.log("CG" + groupId);
    this.groupMemberService.getGroupMembers(groupId).subscribe(
      (data) => {
        const group = this.userGroups.find(g => g.groupId === groupId);
        if (group) {
          const userObservables = data.map(member => this.userService.getUser(member.userId));
          forkJoin(userObservables).subscribe(
            (users) => {
              group.members = users;
              console.log("CG" + JSON.stringify(group));
            },
            (error) => {
              console.error('Error fetching user details', error);
            }
          );
        }
      },
      (error) => {
        console.error('Error fetching group members', error);
      }
    );
  }

  filterUsersNotInGroup(): void {
    const memberIds = this.editGroupMembers.map(member => member.userId);
    this.allUsers = this.allUsers.filter(user => !memberIds.includes(user.userId));
  }

  openCreateGroupModal(): void {
    this.modalRef = this.modalService.open(this.createGroupModal);
  }

  createGroup(): void {
    const userId = Number(localStorage.getItem('userId')); // Assuming userId is stored in localStorage
    const newGroup = { userId, groupName: this.newGroupName, groupId: 0 }; // Initialize groupId to 0 or another appropriate value
    console.log(newGroup);
    this.groupService.postUserGroup(newGroup).subscribe(
      (response) => {
        console.log('Group created successfully', response);
        alert('Group created successfully'); // Show success popup
        this.modalRef.close();
        this.newGroupName = ''; // Reset new group form
        this.fetchUserGroups(); // Refresh the group list
      },
      (error) => {
        console.error('Error creating group', error);
      }
    );
  }

  openEditGroupModal(group: UserGroup): void {
    this.editGroupName = group.groupName;
    this.editGroupId = group.groupId;
    this.editModalRef = this.modalService.open(this.editGroupModal);
    this.fetchEditGroupMembers(group.groupId);
    this.fetchAllUsers();
  }

  fetchEditGroupMembers(groupId: number): void {
    this.groupMemberService.getGroupMembers(groupId).subscribe(
      (data) => {
        const userObservables = data.map(member => this.userService.getUser(member.userId));
        forkJoin(userObservables).subscribe(
          (users) => {
            this.editGroupMembers = users;
            this.filterUsersNotInGroup();
          },
          (error) => {
            console.error('Error fetching user details', error);
          }
        );
      },
      (error) => {
        console.error('Error fetching group members', error);
      }
    );
  }

  editGroup(groupId: number): void {
    const userId = Number(localStorage.getItem('userId')); // Assuming userId is stored in localStorage
    const userGroupDto = { userId, groupId, groupName: this.editGroupName };
    this.groupService.patchUserGroup(groupId, userGroupDto).subscribe(
      () => {
        console.log('Group updated successfully');
        alert('Group updated successfully'); // Show success popup
        this.editModalRef.close();
        this.fetchUserGroups(); // Refresh the group list
      },
      (error) => {
        console.error('Error updating group', error);
      }
    );
  }

  removeUserFromGroup(userId: number, event: Event): void {
    event.stopPropagation(); // Prevent form submission
    if (confirm('Are you sure you want to remove this user from the group?')) {
      this.groupMemberService.deleteUserGroupMember(userId, this.editGroupId).subscribe(
        () => {
          console.log('User removed from group successfully');
          alert('User removed from group successfully'); // Show success popup
          this.fetchEditGroupMembers(this.editGroupId); // Refresh the group members list in the modal
        },
        (error) => {
          console.error('Error removing user from group', error);
        }
      );
    }
  }

  addUserToGroup(userId: number): void {
    const userGroupMemberDto = { userId, groupId: this.editGroupId };
    this.groupMemberService.postUserGroupMember(userGroupMemberDto).subscribe(
      () => {
        console.log('User added to group successfully');
        alert('User added to group successfully'); // Show success popup
        this.fetchEditGroupMembers(this.editGroupId); // Refresh the group members list in the modal
      },
      (error) => {
        console.error('Error adding user to group', error);
      }
    );
  }

  confirmDeleteGroup(groupId: number): void {
    if (confirm('Are you sure you want to delete this group?')) {
      this.groupService.deleteUserGroup(groupId).subscribe(
        () => {
          console.log('Group deleted successfully');
          alert('Group deleted successfully'); // Show success popup
          this.fetchUserGroups(); // Refresh the group list
        },
        (error) => {
          console.error('Error deleting group', error);
        }
      );
    }
  }
}
