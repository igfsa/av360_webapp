import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TurmasComponent } from './turmas/turmas.component';
import { AlunosComponent } from './alunos/alunos.component';
import { NavComponent } from './nav/nav.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    TurmasComponent,
    AlunosComponent,
    NavComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('webav360');
}
