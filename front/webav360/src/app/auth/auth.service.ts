import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { finalize, of, shareReplay, tap, catchError, Observable, map, switchMap } from 'rxjs';
import { API_URL } from '../app.config';
import { Perfil } from '../Models/Perfil';

interface MeResponse {
  autenticado: boolean;
  id: string;
  nome: string;
  perfil: Perfil;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private baseURL = inject(API_URL);

  isLogged = signal<boolean>(false);
  loading = signal<boolean>(true);
  perfil = signal<Perfil | null>(null);
  isAdmin = computed(() => this.perfil() === `Admin`);

  private checkAuth$?: Observable<boolean>;

  public checkAuth() {
    if (this.checkAuth$) return this.checkAuth$;

    this.checkAuth$ = this.http.get<MeResponse>(`${this.baseURL}/api/Autenticacao/Me`, {
      withCredentials: true,
    }).pipe(
      tap(res => this.perfil.set(res.perfil)),
      map(() => true),
      catchError(() => {
        this.perfil.set(null);
        return of(false);
      }),
      tap(isAuth => this.isLogged.set(isAuth)),
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
    this.isLogged.set(false);
    this.perfil.set(null);
    this.checkAuth$ = undefined;
  }
}
