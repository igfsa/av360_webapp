import { Component, HostListener, Inject, TemplateRef, ChangeDetectionStrategy } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

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
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrls: ['./nav.component.scss', '../../app.scss'],
})
export class NavComponent {

  mostrarScrollTop = false;

  constructor(
    private router: Router
    , public authService: AuthService
    , @Inject(NgbOffcanvas) private offcanvasService: NgbOffcanvas
  ) {}

  open(content: TemplateRef<any>) {
    setTimeout(() => this.offcanvasService.open(content));
	}

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  @HostListener('window:scroll')
  onScroll(): void {

    this.mostrarScrollTop =
      window.scrollY > 300;
  }

  public scrollTop(): void {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }
}
