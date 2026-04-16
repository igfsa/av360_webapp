import { Component, inject, TemplateRef  } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Router } from '@angular/router';

import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports:  [
              RouterLink,
              RouterLinkActive,
              RouterOutlet
            ],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', '../../app.scss'],
})
export class NavComponent {
  private offcanvasService = inject(NgbOffcanvas);

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  isPublicRoute(): boolean {
     const aux = this.router.url.includes('avaliacao')
        || this.router.url.includes('login');
    return aux;
  }

  open(content: TemplateRef<any>) {
    setTimeout(() => this.offcanvasService.open(content));
	}

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
