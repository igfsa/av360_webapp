import { Component, HostListener, signal, ChangeDetectionStrategy } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { NavComponent } from './Components/nav/nav.component';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavComponent,
    NgbModule
  ],
  templateUrl: './app.html',
  changeDetection: ChangeDetectionStrategy.Eager,
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
