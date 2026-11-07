import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { finalize, of, shareReplay, tap, catchError, Observable, map, switchMap } from 'rxjs';
import { API_URL } from '../app.config';
import { Me } from '../Models/Me';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseURL = inject(API_URL);

  loading = signal<boolean>(true);
  usuario = signal<Me | null>(null);
  perfil = computed(() => this.usuario()?.perfil ?? null);
  isAdmin = computed(() => this.perfil() === 'Admin');

  private checkAuth$?: Observable<boolean>;

  public checkAuth() {
    if (this.checkAuth$) return this.checkAuth$;

    this.checkAuth$ = this.http.get<Me>(`${this.baseURL}/api/Autenticacao/Me`, {
      withCredentials: true,
    }).pipe(
      tap(me => this.usuario.set(me)),
      map(() => true),
      catchError(() => {
        this.usuario.set(null);
        return of(false);
      }),
      finalize(() => this.loading.set(false)),
      shareReplay(1)
    );

    return this.checkAuth$;
  }

  public login(userName: string, senha: string) {
    return this.http.post(`${this.baseURL}/api/Autenticacao/Login`,
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
    this.http.post(`${this.baseURL}/api/Autenticacao/Logout`, {}, {
      withCredentials: true
    }).subscribe();
    this.usuario.set(null);
    this.checkAuth$ = undefined;
  }
}
