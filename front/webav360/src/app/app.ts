import { Component, HostListener, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavComponent } from './Components/nav/nav.component';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavComponent,
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  protected readonly title = signal('webav360');

  hydrated = signal(false);
  mostrarScrollTop = false;

  constructor(
    protected authService: AuthService
  ) {
      queueMicrotask(() => {
        this.hydrated.set(true);
      });
    }

  ngOnInit() {
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
