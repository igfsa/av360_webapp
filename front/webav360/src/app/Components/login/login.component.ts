import { Component, OnInit, signal } from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { AuthService } from '../../auth/auth.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FormsHelper } from '../../Helpers/formsHelper';

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

  loginForm = form(this.loginModel, schemaPath => {
    required(schemaPath.userName, {message: `Usuário deve ser informado`});
    required(schemaPath.senha, {message: `Senha deve ser informada`});
  });

  constructor(
    private authService: AuthService,
    private router: Router,
    private formHekper: FormsHelper
  ) {}

  ngOnInit(): void {
  }

  login(): void {
    this.formHekper.markAllTouched(this.loginForm);
    if (this.loginForm().invalid())
    {
      Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.onmouseenter = Swal.stopTimer;
          toast.onmouseleave = Swal.resumeTimer;
        }
      }).fire({
        icon: 'error',
        title: 'Erro',
        text: `Insira seu usuário e senha`
      });
      return
    }
    this.authService.login(this.loginModel().userName, this.loginModel().senha).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Dados Inválidos`
        });
      }
    });
  }

}
