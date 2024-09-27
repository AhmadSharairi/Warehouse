import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ValidateFormHelper } from '../login/ValidateFormHelper';
import { WarehouseService } from './warehouse.service';
import { Warehouse } from '../../shared/models/Warehouse';
import { WarehouseInfo } from '../../shared/models/WarehouseInfo';
import { Item } from '../../shared/models/Item';
import { Router } from '@angular/router';

@Component({
  selector: 'app-warehouse',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './warehouse.component.html',
  styleUrls: ['./warehouse.component.css'],
})
export class WarehouseComponent implements AfterViewInit, OnInit {
  public warehousForm!: FormGroup;
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
    this.createCheckoutForm();
    this.getAllWarehouses();
    this.loadWarehouses();
  }

  createCheckoutForm() {
    this.warehousForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]],
      address: ['', [Validators.required]],
      cityName: ['', [Validators.required]],
      countryName: ['', [Validators.required]],
    });
  }

  isInvalid(controlName: string): boolean {
    const control = this.warehousForm.get(controlName);
    return !!(control?.invalid && (control.dirty || control.touched));
  }

  WarehouseSubmit() {
    if (this.warehousForm.valid) {
      this.saveWarehouse();
    } else {
      ValidateFormHelper.validateAllFormFields(this.warehousForm); 
    }
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

  saveWarehouse() {
    if (this.warehousForm.valid) {
      const newWarehouse: Warehouse = this.warehousForm.value;

      console.log(newWarehouse);

      this.warehouseService.addWarehouse(newWarehouse).subscribe({
        next: (createdWarehouse) => {
          this.toastr.success('Warehouse added successfully');
          this.warehousForm.reset(); 
        },
        error: (err: any) => {
          console.error('Error adding warehouse:', err);
          this.toastr.error('Error adding warehouse. Please try again.');
        },
      });
    } else {
      this.toastr.error('Please fill out all required fields.');
    }
  }
}
