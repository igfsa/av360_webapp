import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(): boolean {
    const token = localStorage.getItem('token');
    console.log(token);

    if (!token) {
      console.log('sem token: ', token);
      this.router.navigate(['/login']);
      return false;
    }

    try {
      const decoded: any = jwtDecode(token);
      const exp = decoded.exp * 1000;

      if (Date.now() > exp) {
        localStorage.removeItem('token');
        this.router.navigate(['/login']);
        return false;
      }

      return true;
    } catch {
      localStorage.removeItem('token');
      this.router.navigate(['/login']);
      return false;
    }
  }
}
