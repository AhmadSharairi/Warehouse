import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { WarehouseService } from '../warehouse/warehouse.service';
import { UserService } from '../user/user.service';
import { AuthService } from '../login/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  warehouseCount: number = 0;
  UserCount: number = 0;
  ItemsCount: number = 0;

  constructor(
    private warehouseService: WarehouseService,
    private userService: UserService,
    private authService: AuthService,
    private route: Router
  ) {}
  ngOnInit(): void {
    this.getWarehouseCount();
    this.getUserCount();
    this.getTotalItemsCount();
  }

  getWarehouseCount() {
    this.warehouseService
      .getAllWarehouses()
      .subscribe(
        (data: any) => (this.warehouseCount = data?.$values?.length || 0)
      );
  }

  getUserCount() {
    this.userService.getAllUsers().subscribe((data: any) => {
      this.UserCount = data.length || 0;
      console.log(data);
    });
  }

  getTotalItemsCount(): void {
    this.warehouseService.getTotalItemsCount().subscribe(
      (count: number) => {
        this.ItemsCount = count;
        console.log('Total Items Count:', this.ItemsCount);
      },
      (error) => {
        console.error('Error fetching total items count:', error);
      }
    );
  }

  logout() {
    this.authService.SignOut();
  }
  goWarehousPage() {
    this.route.navigate(['/warehouse']);
  }

  goUsersPage() {
    this.route.navigate(['/users']);
  }
}
