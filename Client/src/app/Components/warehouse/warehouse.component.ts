import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder,  ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { WarehouseService } from './warehouse.service';
import { WarehouseInfo } from '../../shared/models/WarehouseInfo';
import { Router } from '@angular/router';

@Component({
  selector: 'app-warehouse',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css'],
})
export class WarehouseComponent implements AfterViewInit, OnInit {
  loading: boolean = false;
  warehouseCount: number = 0;
  warehouses: WarehouseInfo[] = [];



  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private toastr: ToastrService ,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.loading = false;
  }

  ngOnInit() {
    this.getAllWarehouses();
    this.loadWarehouses();
  }



  getAllWarehouses() {
    this.warehouseService.getAllWarehouses().subscribe(
      (data: any) => {
        if (data && data.$values) {
          this.warehouseCount = data.$values.length;
          console.log(this.warehouseCount);
          this.warehouses = data.$values;
        } else {
          console.warn('No warehouse data found.');
        }
      },
      (error: any) => {
        console.error('Error fetching warehouses:', error);
        this.toastr.error('Error fetching warehouses. Please try again.');
      }
    );
  }



  loadWarehouses(): void {
    this.warehouseService.getAllWarehouseInfo().subscribe(
      (data :any) => {
        console.log('API response:', data);
        if (data && data.$values) {
          this.warehouses = data.$values;

        } else {
          this.warehouses = [];
        }
        this.loading = false;
      },
      (error) => {
        console.error('Error fetching Items warehouse data:', error);
        this.loading = false;
      }
    );
  }

  checkItems(warehouseId: number) {
    this.router.navigate(['/warehouses', warehouseId]);
  }
  addWarehouse()
  {
    this.router.navigate(['/add-warehouse']);
  }

}
