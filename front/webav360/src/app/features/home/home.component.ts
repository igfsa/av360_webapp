import { Component } from '@angular/core';

import { AuthService } from '@core/auth/auth.service';
import { LoadingComponent } from '@app/shared/components/loading/loading.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    LoadingComponent,
    RouterLink,
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss', '../../app.scss'],
})
export class HomeComponent {

  public loading: boolean = true;

  constructor(
    public authService: AuthService,
  ) {}

  ngOnInit(): void {
    if (this.authService.usuario()){
      this.loading = false;
    }
  }
}
