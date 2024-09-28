import { AfterViewInit, Component, OnInit } from '@angular/core';
import { WarehouseInfo } from '../../shared/models/WarehouseInfo';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { WarehouseService } from '../warehouse/warehouse.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Warehouse } from '../../shared/models/Warehouse';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-warehouse',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-warehouse.component.html',
  styleUrl: './add-warehouse.component.css'
})
export class AddWarehouseComponent implements AfterViewInit, OnInit {
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
          this.router.navigate(['/warehouse']).then(() =>
          {
            window.location.reload();
          });
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
