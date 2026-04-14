import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { _GlobalVariablesService } from '../Service/_GlobalVariables.service';
import { baseURL } from '../../main.server';
import { Professor } from '../Models/Professor';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http :HttpClient) { }

  get token() {
    return localStorage.getItem('token');
  }

  public login(userName: string, senha: string): Observable<any> {
    return this.http.post<any>(`${baseURL}api/Autenticacao/Login`, {userName, senha});
  }

  public signin(professor: Professor): Observable<Professor> {
    return this.http.post<Professor>(`${baseURL}api/Autenticacao/Register`, professor);
  }

  isLogged(): boolean {
   return !!this.token;
  }

  logout() {
    localStorage.removeItem('token');
  }

}
