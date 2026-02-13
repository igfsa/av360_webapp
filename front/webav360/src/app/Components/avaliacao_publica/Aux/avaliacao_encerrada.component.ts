import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'avaliacao-encerrada',
  standalone: true,
  imports: [],
  template: `
    <div class="container">
      <h1>Avaliação encerrada</h1>
    </div>
  `
})
export class AvaliacaoEncerradaComponent implements OnInit {

  constructor() {}

  ngOnInit(): void {
  }
}
