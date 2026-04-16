import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from '../Service/_GlobalVariables.service';
import { Professor } from '../Models/Professor';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _token = signal<string | null>(this.getStoredToken());

  isLogged = signal(!!this._token());

  constructor(private http :HttpClient) { }

  private getStoredToken(): string | null {
    return typeof window !== 'undefined'
      ? localStorage.getItem('token')
      : null;
  }

  get token(): string | null {
    return this._token();
  }

  public login(userName: string, senha: string) {
    return this.http.post<any>(`/api/Autenticacao/Login`, { userName, senha });
  }

  public setSession(token: string) {
    if (typeof window !== 'undefined') {
      localStorage.setItem('token', token);
    }

    this._token.set(token);
    this.isLogged.set(true);
  }


  public signin(professor: Professor): Observable<Professor> {
    return this.http.post<Professor>(`/api/Autenticacao/Register`, professor);
  }

  public logout() {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('token');
    }

    this._token.set(null);
    this.isLogged.set(false);
  }

}
