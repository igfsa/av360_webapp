import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal-layout',
  standalone: true,
  imports: [],
  template: `
  <form
    class="modal-layout">

    <div class="modal-body">
      <ng-content></ng-content>
    </div>

    <div class="modal-footer">

      <button
        class="btn btn-danger"
        type="button"
        (click)="cancelar.emit()">

        {{ cancelarTexto }}

      </button>

      <button
        class="btn btn-success"
        type="button"
        (click)="confirmar.emit()">

        {{ confirmarTexto }}

      </button>

    </div>

  </form>
  `,
  styles: `
    :host {
      display: block;
      height: 100%;
    }

    .modal-layout {
      height: 100%;

      display: flex;
      flex-direction: column;
    }

    .modal-body {
      flex: 1;

      overflow-y: auto;
      min-height: 0;

      padding-right: .5rem;
    }

    .modal-footer {
      flex-shrink: 0;

      display: flex;
      justify-content: end;
      gap: .75rem;

      padding-top: 1rem;
      margin-top: 1rem;

      border-top: 1px solid var(--p-content-border-color);
      background: var(--p-content-background);
    }
  `,
})
export class ModalLayoutComponent {
  @Input() confirmarTexto = 'Confirmar';
  @Input() cancelarTexto = 'Cancelar';

  @Output() confirmar = new EventEmitter<void>();
  @Output() cancelar = new EventEmitter<void>();
}
