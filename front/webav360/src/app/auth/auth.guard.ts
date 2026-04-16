import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router,
    private authService: AuthService
  ) {}

  canActivate(): boolean {
    const token = this.authService.token;

    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }

    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded.exp * 1000;

      if (Date.now() > exp) {
        this.authService.logout();
        return false;
      }

      return true;
    } catch {
      this.authService.logout();
      return false;
    }
  }
}
