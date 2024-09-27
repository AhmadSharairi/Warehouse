import { Component, OnInit } from '@angular/core';
import { AuthService } from '../login/auth.service';
import { ResetPassword } from '../../shared/models/ResetPassword';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../user/user.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export class ChangePasswordComponent implements OnInit {
  userId: number | undefined;
  passwordVisible: boolean = false;

  changePasswordForm: FormGroup;
  userData: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private userService: UserService,
    private toastr: ToastrService
  ) {
    this.changePasswordForm = this.fb.group(
      {
        currentPassword: ['', [Validators.required]],
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      { validator: this.passwordMatchValidator }
    );
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.userId = idParam ? +idParam : undefined;
  }

  getUser() {
    const id = this.userId ?? 0;
    this.userData = this.userService.getUserById(id);
  }

  togglePasswordVisibility() {
    this.passwordVisible = !this.passwordVisible;
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('newPassword')?.value === form.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  onSubmit() {
    if (this.changePasswordForm.valid) {
      const resetPasswordDto: ResetPassword = {
        ...this.changePasswordForm.value,
        Id: this.userId!,
      };

      this.authService.resetPassword(resetPasswordDto).subscribe({
        next: (response: any) => {
          console.log('Password changed successfully!', response);
          this.toastr.success('Password changed successfully!', 'Success');
        },
        error: (err: any) => {
          console.error('Error changing password', err);
          this.toastr.error(
            'Error changing password. Please try again.',
            'Error'
          );
        },
      });
    } else {
      this.toastr.warning(
        'Please fill in all required fields correctly.',
        'Warning'
      );
    }
  }
}
