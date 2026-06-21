import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { finalize, of, shareReplay, tap, catchError, Observable, map, switchMap } from 'rxjs';

import { baseURL } from '../../main.server';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  isLogged = signal<boolean>(false);
  loading = signal<boolean>(true);

  private checkAuth$?: Observable<boolean>;

  public checkAuth() {
    if (this.checkAuth$) return this.checkAuth$;

    this.checkAuth$ = this.http.get(`${baseURL}/api/Autenticacao/Me`, {
      withCredentials: true,
      observe: 'response'
    }).pipe(
      map(() => true),
      catchError(() => of(false)),
      tap(isAuth => this.isLogged.set(isAuth)),
      finalize(() => this.loading.set(false)),
      shareReplay(1)
    );

    return this.checkAuth$;
  }

  public login(userName: string, senha: string) {
    return this.http.post(`${baseURL}/api/Autenticacao/Login`,
      { userName, senha },
      { withCredentials: true }
    ).pipe(
      switchMap(() => {
        this.checkAuth$ = undefined;
        return this.checkAuth();
      })
    );
  }

  public logout() {
    this.http.post(`${baseURL}/api/Autenticacao/Logout`, {}, {
      withCredentials: true
    }).subscribe();
    this.isLogged.set(false);
    this.checkAuth$ = undefined;
  }
}
