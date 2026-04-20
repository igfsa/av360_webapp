import { Component, inject, PLATFORM_ID, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { NavComponent } from './Components/nav/nav.component';
import { AuthService } from './auth/auth.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavComponent,
    NgbModule
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App {
  protected readonly title = signal('webav360');

  hydrated = signal(false);

  constructor(protected authService: AuthService) {
    const platformId = inject(PLATFORM_ID);

      queueMicrotask(() => {
        this.hydrated.set(true);
      });
    }

  ngOnInit() {
  }
}
