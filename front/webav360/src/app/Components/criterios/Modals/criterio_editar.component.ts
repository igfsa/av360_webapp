import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Criterio } from '../../../Models/Criterio';


@Component({
  selector: 'app-criterio-editar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-header">
      <h4 class="modal-title" style = "font-size: 2.4rem;">Critério </h4>
      <span> {{ criterio.nome }}</span>
    </div>

    <div class="modal-body">
      <div class="input-group mb-3 row">
        <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Código: </span>
        <input type="text" class="form-control" [(ngModel)]="criterioEdit.nome" aria-label="Nome" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
      </div>
    </div>

    <div class="modal-footer">
      <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
      <button class="btn btn-secondary btn-success" (click)="salvar()">Salvar</button>
    </div>
  `
})
export class CriterioEditarModalComponent implements OnInit {

  @Input() criterio!: Criterio;

  criterioEdit!: Criterio;

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
    this.criterioEdit = { ...this.criterio };
  }

  salvar(): void {
    this.modal.close(this.criterioEdit);
  }
}
