import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, min, max, required } from '@angular/forms/signals';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../../Models/Turma';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormField,
  ],
  template: `
  <div class="modal-header mx-5 mt-5">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Nova Turma</h4>
  </div>

  <div class="modal-body vh-100 mx-5" >
    <form novalidate>
      <div class="input-group mb-3 row">
        <span class="input-group-text col-2" style = "font-size: 1.6rem;">Código: </span>
        <input type="text" class="form-control" [formField]="turmaForm.cod" aria-label="Cod" style = "font-size: 1.6rem;">
        @if (turmaForm.cod().touched() && (turmaForm.cod().invalid() || turmaForm.cod().value() === '')) {
          <div class="alert alert-danger">
            <span class="text-danger fw-bold" >Cod inválido...</span>
          </div>
        }
      </div>
      <div class="input-group mb-3 row" >
        <span class="input-group-text col-2" style = "font-size: 1.6rem;">Nota Máxima: </span>
        <input type="number" class="form-control" [formField]="turmaForm.notaMax" aria-label="Nota Máxima" style = "font-size: 1.6rem;">
        @if (turmaForm.cod().touched() && (turmaForm.notaMax().value() < 1 || turmaForm.notaMax().value() > 100 || turmaForm.cod().value() === '')){
          <div class="alert alert-danger">
            <span class="text-danger fw-bold" >Nota deve ser entre 1 e 100...</span>
          </div>
        }
      </div>
    </form>
  </div>

  <div class="modal-footer align-bottom mx-5 mb-5" >
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvarImport()" [disabled]="!podeSalvar">Salvar e Importar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()" [disabled]="!podeSalvar">Salvar</button>
  </div>
  `
})
export class TurmaCriarModalComponent implements OnInit {

  turmaModel = signal<Turma>({
    id: 0,
    cod: '',
    notaMax: 0
  })

  turmaForm = form(this.turmaModel);

  get podeSalvar(): boolean {
    if (this.turmaForm.notaMax().value() < 1 || this.turmaForm.notaMax().value() > 100 || this.turmaForm.cod().value() === '')
      return false;
    if (!this.turmaForm.cod || this.turmaForm.cod().value() === '')
      return false;
    return true;
  }

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
  }

  salvar(): void {
    this.modal.close({Turma: this.turmaForm(), ImportAlunos: false});
  }

  salvarImport(): void {
    this.modal.close({Turma: this.turmaModel(), ImportAlunos:  true});
  }
}
