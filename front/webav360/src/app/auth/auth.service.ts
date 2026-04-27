import { HttpClient } from '@angular/common/http';
import { inject, Injectable, makeStateKey, PLATFORM_ID, signal, TransferState } from '@angular/core';
import { finalize, of, shareReplay, tap, catchError, Observable, map, switchMap } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

const AUTH_KEY = makeStateKey<boolean>('auth');

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);

  isLogged = signal<boolean>(false);
  loading = signal<boolean>(true);

  private checkAuth$?: Observable<boolean>;

  public checkAuth() {
    console.log('checkAuth chamado');
    if (this.checkAuth$) return this.checkAuth$;

    this.checkAuth$ = this.http.get('/api/Autenticacao/Me', {
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
    return this.http.post('/api/Autenticacao/Login',
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
    this.http.post('/api/Autenticacao/Logout', {}, {
      withCredentials: true
    }).subscribe();
    this.isLogged.set(false);
    this.checkAuth$ = undefined;
  }
}
