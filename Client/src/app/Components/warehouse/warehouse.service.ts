import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Warehouse } from '../../shared/models/Warehouse';
import { Item } from '../../shared/models/Item';
import { City } from '../../shared/models/City';
import { Country } from '../../shared/models/Country';
import { WarehouseInfo } from '../../shared/models/WarehouseInfo';

@Injectable({
  providedIn: 'root',
})
export class WarehouseService {
  private apiUrl = 'http://localhost:5152/api/Warehouse';


  constructor(private http: HttpClient) {}

  
  getAllWarehouses(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(this.apiUrl);
  }

  getWarehouseById(id: number): Observable<Warehouse> {
    return this.http.get<Warehouse>(`${this.apiUrl}/${id}`);
  }

  
  addWarehouse(warehouse: Warehouse): Observable<Warehouse>
  {
    return this.http.post<Warehouse>(this.apiUrl, warehouse);
  }

  getAllWarehouseInfo(): Observable<WarehouseInfo[]> {
    return this.http.get<WarehouseInfo[]>(`${this.apiUrl}/info`);
  }


  getItemsByWarehouseId(warehouseId: number): Observable<Item[]> {
    const url = `${this.apiUrl}/${warehouseId}/items`;
    return this.http.get<Item[]>(url);
  }

  getTotalItemsCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/items/count`);
  }






}
