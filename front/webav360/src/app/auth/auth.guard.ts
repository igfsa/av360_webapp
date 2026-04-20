import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { catchError, map, Observable, of } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  private platformId = inject(PLATFORM_ID);

  constructor(private router: Router,
    private authService: AuthService
  ) {}

  canActivate(): Observable<boolean> {


    if (!isPlatformBrowser(this.platformId)) {
      return of(true);
    }

    if (this.authService.isLogged()) {
      return of(true);
    }

    return this.authService.checkAuth().pipe(
      map(() => true),
      catchError(() => {
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }
}
