import { Component, OnInit, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../auth/auth.service';
import { FormsHelper } from '../../Helpers/formsHelper';
import { AlertService } from '../shared/alert/alert.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
   ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss', '../../app.scss'],
})
export class LoginComponent implements OnInit {

  loginModel = signal({
    userName: '',
    senha: ''
  })

  ocultarSenha = signal<boolean>(true);

  loginForm = form(this.loginModel, schemaPath => {
    required(schemaPath.userName, {message: `Usuário deve ser informado`});
    required(schemaPath.senha, {message: `Senha deve ser informada`});
  });

  constructor(
    private authService: AuthService,
    private router: Router,
    private formHekper: FormsHelper,
    private alert: AlertService,
  ) {}

  ngOnInit(): void {
  }

  login(): void {
    this.formHekper.markAllTouched(this.loginForm);
    if (this.loginForm().invalid())
    {
      this.alert.toastError(`Insira seu usuário e senha`);
      return
    }
    this.authService.login(this.loginModel().userName, this.loginModel().senha).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: err => {
        this.alert.toastError(err.error?.message ?? `Dados Inválidos`);
      }
    });
  }
}
