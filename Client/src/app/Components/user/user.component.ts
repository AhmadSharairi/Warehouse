import { Component, OnInit } from '@angular/core';
import { UserService } from './user.service';
import { ToastrService } from 'ngx-toastr';
import { UserInfo } from '../../shared/models/UserInfo';
import { CommonModule } from '@angular/common';
import { AuthService } from '../login/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  users: UserInfo[] = [];
  roleName: string = "";

  constructor(
    private userService: UserService,
    private toastr: ToastrService,
    private loginService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getAllUsers();
    this.getRoleByToken();
  }

  getRoleByToken() {
    this.roleName = this.loginService.getRoleFromToken();
    return this.roleName;
  }

  getAllUsers() {
    this.userService.getAllUsers().subscribe(
      (data: UserInfo[]) => {
        this.users = data;
        console.log(this.users);
      },
      (error: any) => {
        console.error('Error fetching users:', error);
        this.toastr.error('Error fetching users. Please try again.');
      }
    );
  }

  editUser(userId: number) {
    this.router.navigate(['/user', userId, 'update-user']);
  }

  changePassword(userId: number) {
    this.router.navigate(['/user', userId, 'change-password']);
  }

  deleteUser(id: number) {
    const confirmDelete = confirm('Are you sure you want to delete this user?');

    if (confirmDelete) {
      this.userService.deleteUser(id).subscribe(
        () => {
          this.toastr.success('User deleted successfully.');
          this.getAllUsers();
        },
        (error) => {
          console.error('Error deleting user:', error);
          this.toastr.error('Error deleting user. Please try again.');
        }
      );
    }
  }

  toggleUserStatus(user: UserInfo) {
    user.isActive = !user.isActive;

    this.userService.updateUser(user.id, user).subscribe(
      () => {
        this.toastr.success(`User has been ${user.isActive ? 'enabled' : 'disabled'}`);
      },
      (error) => {
        console.error('Error updating user status:', error);
        this.toastr.error('Error updating user status. Please try again.');
        user.isActive = !user.isActive; 
      }
    );
  }
}
