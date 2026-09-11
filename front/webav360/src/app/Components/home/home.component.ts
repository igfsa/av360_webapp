import { Component } from '@angular/core';

import { AuthService } from '../../auth/auth.service';
import { LoadingComponent } from '../shared/loading/loading.component';
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
    if (this.authService.isLogged()){
      this.loading = false;
    }
  }
}
