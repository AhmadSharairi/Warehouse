import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../Components/login/auth.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const toast = inject(ToastrService);

  if (authService.isLoggedIn()) {
    return true;
  } else {
    toast.error('Please Login First!');
    router.navigate(['/login']);
    return false;
  }
};
