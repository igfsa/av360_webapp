import { Component, OnInit, signal } from '@angular/core';
import { form, FormField } from '@angular/forms/signals';
import { AuthService } from '../../auth/auth.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
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

  loginForm = form(this.loginModel);

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
  }

  login(): void {
    this.authService.login(this.loginModel().userName, this.loginModel().senha).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: err.error?.message ?? `Erro ao acessar`
        });
      }
    });
  }

}
