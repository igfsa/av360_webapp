import { HttpInterceptorFn } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const authService = inject(AuthService);

  let authReq = req;

  const token = authService.token;
  const isBrowser = typeof window !== 'undefined';

  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(authReq).pipe(
    catchError(err => {
      const publicRoutes = [
        'login',
        'avaliacao'
      ];

      const isPublic = publicRoutes.some(route =>
        req.url.includes(route)
      ) && isBrowser;

      if (err.status === 401 && !isPublic) {
        authService.logout();
        router.navigate(['/login']);
      }
      return throwError(() => err);
    })
  );
};
