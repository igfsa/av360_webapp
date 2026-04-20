import { HttpBackend, HttpClient, HttpInterceptorFn } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const authService = inject(AuthService);
  const backend = inject(HttpBackend);
  const http = new HttpClient(backend);
  const platformId = inject(PLATFORM_ID)

  if (!isPlatformBrowser(platformId)) {
    return next(req);
  }

  const authReq = req.clone({
    withCredentials: true
  });

  return next(authReq).pipe(
    catchError(err => {

      const publicRoutes = [
        'login',
        'avaliacao'
      ];

      const isPublic = publicRoutes.some(route =>
        req.url.includes(route)
      );

      const isRefresh = req.url.includes('/Autenticacao/Refresh');

      if (err.status === 401 && !isPublic && !isRefresh) {
        return http
          .post('/api/Autenticacao/Refresh', {}, { withCredentials: true })
          .pipe(
            switchMap(() => next(authReq)),
              catchError(refreshErr => {
                authService.logout();
                router.navigate(['/login']);
                return throwError(() => refreshErr);
              })
          );
      }
      return throwError(() => err);
    })
  );
};
