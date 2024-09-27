import { Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { authGuard } from './shared/guards/auth.guard';
import { WarehouseComponent } from './Components/warehouse/warehouse.component';
import { UserComponent } from './Components/user/user.component';
import { HomeComponent } from './Components/home/home.component';
import { WarehouseItemsComponent } from './Components/warehouse-items/warehouse-items.component';
import { ChangePasswordComponent } from './Components/change-password/change-password.component';
import { UpdateUserComponent } from './Components/update-user/update-user.component';




export const routes: Routes = [
  { path: '', component: LoginComponent },
  {  path: 'home', component: HomeComponent, canActivate: [authGuard]},
  {  path: 'warehouse', component: WarehouseComponent, canActivate: [authGuard]},
  {  path: 'warehouses/:id', component: WarehouseItemsComponent, canActivate: [authGuard]},
  {  path: 'user/:id/change-password', component: ChangePasswordComponent, canActivate: [authGuard]},
  {  path: 'user/:id/update-user', component: UpdateUserComponent, canActivate: [authGuard]},
  {  path: 'users', component: UserComponent, canActivate: [authGuard]},
];
