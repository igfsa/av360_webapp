import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, max, maxLength, min, required } from '@angular/forms/signals';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Aluno } from '../../../Models/Aluno';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { FormsHelper } from '../../../Helpers/formsHelper';

@Component({
  selector: 'app-aluno-turma-add-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
  ],
  changeDetection: ChangeDetectionStrategy.Eager,
  template: `
  <div class="modal-header">
    <h1 class="modal-title">Adicionar Aluno</h1>
  </div>

  <form (ngSubmit)="salvar()" class="d-flex flex-column vh-100">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Nome: </label>
      <input type="text" class="form-control" [formField]="alunoForm.nome" aria-label="Nome">
      <small [class.text-danger]="alunoForm.nome().value().length >= 100">
        {{ alunoForm.nome().value().length }}/100
      </small>
      @if (alunoForm.nome().touched() && alunoForm.nome().invalid()) {
        <ul class="error-list">
          @for (error of alunoForm.nome().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
    </div>

    <div class="modal-footer mt-auto" >
      <button class="btn btn-secondary btn-danger" type="button" (click)="cancelar($event)">Cancelar</button>
      <button class="btn btn-secondary btn-success" type="submit" >Salvar</button>
    </div>
  </form>
  `
})
export class AlunoTurmaAddModalComponent implements OnInit {

  alunoModel = signal<Aluno>({
    id: 0,
    nome: ''
  })

  alunoForm = form(this.alunoModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Aluno deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Nome do Aluno não pode ter mais de 100 caracteres.'});
  });

  constructor(public modal: NgbActiveModal,
    private formHelper: FormsHelper) {}

  ngOnInit(): void {
  }

  public cancelar(event: MouseEvent): void {
    event.preventDefault();
    event.stopPropagation();

    this.modal.dismiss('cancelar')
  }

  public salvar(): void {
    this.formHelper.markAllTouched(this.alunoForm);

    if (this.alunoForm().invalid())
    {
      Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        didOpen: (toast) => {
          toast.onmouseenter = Swal.stopTimer;
          toast.onmouseleave = Swal.resumeTimer;
        }
      }).fire({
        icon: 'error',
        title: 'Erro',
        text: `Verifique as informações do Aluno`
      });
      return
    }

    this.modal.close(this.alunoModel());
  }
}

