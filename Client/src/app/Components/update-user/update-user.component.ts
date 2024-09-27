import { Component, OnInit } from '@angular/core';
import { UserService } from '../user/user.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-user',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './update-user.component.html',
  styleUrls: ['./update-user.component.css'] 
})
export class UpdateUserComponent implements OnInit {
  updateUserForm!: FormGroup;
  userId!: number; 
  roles: string[] = ['Admin', 'Management', 'Auditor'];

  constructor(
    private fb: FormBuilder, 
    private userService: UserService, 
    private route: ActivatedRoute ,
    private router: Router 
  ) {}

  ngOnInit(): void {
    this.userId = +this.route.snapshot.paramMap.get('id')!; 


    this.updateUserForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      fullName: ['', Validators.required],
      roleId: ['', Validators.required],
      isActive: [false] ,
      
    });


    this.loadUserDetails();
  }

  loadUserDetails() {
    this.userService.getUserById(this.userId).subscribe(user => {
      this.updateUserForm.patchValue({
        ...user,
        id: this.userId 
      });
    });
  }
  


  onSubmit() {
    if (this.updateUserForm.valid) {
      const userData = { ...this.updateUserForm.value, id: this.userId }; 
      this.userService.updateUser(this.userId, userData).subscribe(() => {
        alert('User updated successfully');
        this.router.navigate(['/users']);
      
      });
    }
  }
  
  
}
