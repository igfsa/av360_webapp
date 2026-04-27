import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField } from '@angular/forms/signals';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Aluno } from '../../../Models/Aluno';

@Component({
  selector: 'app-aluno-turma-add-modal',
  standalone: true,
  imports: [CommonModule, FormField],
  template: `
  <div class="modal-header mx-5 mt-5">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Adicionar Aluno</h4>
  </div>

  <div class="modal-body vh-100 mx-5" >
    <form novalidate>
      <div class="input-group mb-3 row">
        <span class="input-group-text col-2" style = "font-size: 1.6rem;">Nome: </span>
        <input type="text" class="form-control" [formField]="alunoForm.nome" aria-label="Nome" style = "font-size: 1.6rem;">
        @if (alunoForm.nome().touched() && (alunoForm.nome().invalid() || alunoForm.nome().value() === '')) {
          <div class="alert alert-danger">
            <span class="text-danger fw-bold" >Nome inválido...</span>
          </div>
        }
      </div>
    </form>
  </div>

  <div class="modal-footer align-bottom mx-5 mb-5" >
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success"
      (click)="salvar()"
      [disabled]="!this.alunoForm.nome || this.alunoForm.nome().value() === '' || this.alunoForm.nome().invalid()"
    >
      Salvar
    </button>
  </div>
  `
})
export class AlunoTurmaAddModalComponent implements OnInit {

  alunoModel = signal<Aluno>({
    id: 0,
    nome: ''
  })

  alunoForm = form(this.alunoModel);

  get podeSalvar(): boolean {
    if (!this.alunoForm.nome || this.alunoForm.nome().value() === '' || this.alunoForm.nome().invalid())
      return false;
    return true;
  }

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
  }

  salvar(): void {
    this.modal.close(this.alunoModel());
  }
}
