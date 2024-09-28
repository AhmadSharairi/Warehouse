import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastModule } from 'ng-angular-popup';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, HttpClientModule , NgToastModule ],

  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  type: string = 'password';
  isText = false;
  eyeIcon: string = 'fa-eye-slash';
  public resetPasswordEmail!: string;
  public isValidEmail!: boolean;
  isLoading: boolean = false;



  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.eyeIcon = this.isText ? 'fa-eye' : 'fa-eye-slash';
    this.type = this.isText ? 'text' : 'password';
  }


  onLogin() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.auth.loginPost(this.loginForm.value).subscribe({
        next: (res: { accessToken: any; refreshToken: any; message: any }) => {
          this.isLoading = false;
          this.loginForm.reset();
          this.auth.storeToken(res.accessToken);
          this.auth.storeRefreshToken(res.refreshToken);
          this.toastr.success('Login Successfully');
          this.router.navigate(['/home']);
        },
        error: (err: any) => {
          this.isLoading = false;
          this.toastr.error('Email or password not correct!');
          this.toastr.error('Account is disabled and needs to contact support');
          console.log(err);
        },
      });
    }
  }


}
