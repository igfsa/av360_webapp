import { Component, HostListener, Inject, TemplateRef } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { Drawer, DrawerModule } from 'primeng/drawer';

import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports:  [
              RouterLink,
              RouterLinkActive,
              RouterOutlet,
              DrawerModule,
              Drawer,
            ],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', '../../app.scss'],
})
export class NavComponent {

  mostrarScrollTop: boolean = false;
  visible: boolean = false;

  constructor(
    private router: Router
    , public authService: AuthService
  ) {}

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
