import { Component, signal, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, min, max, required, maxLength } from '@angular/forms/signals';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../../Models/Turma';
import { FormsModule } from '@angular/forms';
import { FormsHelper } from '../../../Helpers/formsHelper';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
  ],
  template: `
  <div class="modal-header">
    <h1 class="modal-title" >Nova Turma</h1>
  </div>

  <form (ngSubmit)="salvar()" class="d-flex flex-column vh-100">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Nome da Turma: </label>
      <input type="text" class="form-control" [formField]="turmaForm.cod" aria-label="Cod" >
      <small [class.text-danger]="turmaForm.cod().value().length >= 100">
        {{ turmaForm.cod().value().length }}/100
      </small>
      @if (turmaForm.cod().touched() && turmaForm.cod().invalid()) {
        <ul class="error-list">
          @for (error of turmaForm.cod().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
      <label>Nota Máxima: </label>
      <input type="number" class="form-control" [formField]="turmaForm.notaMax" aria-label="Nota Máxima" >
      @if (turmaForm.notaMax().touched() && turmaForm.notaMax().invalid()){
        <ul class="error-list">
          @for (error of turmaForm.notaMax().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
      <label style="display: inline-block">
        <input type="checkbox" class="form-check-input" [formField]="turmaForm.importarAlunos" aria-label="Importar Alunos">
        Importar alunos após salvar
      </label>
    </div>

    <div class="modal-footer mt-auto" >
      <button class="btn btn-secondary btn-danger" type="button" (click)="cancelar($event)">Cancelar</button>
      <button class="btn btn-secondary btn-success" type="submit" >Salvar</button>
    </div>
  </form>
  `
})
export class TurmaCriarModalComponent implements OnInit {

  turmaModel = signal<Turma & {importarAlunos: boolean}>({
    id: 0,
    cod: '',
    notaMax: 0,
    importarAlunos: true
  })

  turmaForm = form(this.turmaModel, schemaPath => {
    required(schemaPath.cod, {message: `Nome da Turma deve ser inserido`})
    maxLength(schemaPath.cod, 100, {message: 'Nome da Turma não pode ter mais de 100 caracteres.'});

    required(schemaPath.notaMax, {message: `Nota Máxima da turma deve ser inserida`});
    min(schemaPath.notaMax, 1, {message: `Nota Máxima não pode ser menor que 1`});
    max(schemaPath.notaMax, 100, {message: `Nota Máxima não pode ser maior que 100`});
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
    this.formHelper.markAllTouched(this.turmaForm);

    if (this.turmaForm().invalid())
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
        text: `Verifique os dados da Turma`
      });
      return
    }

    const model = this.turmaModel();

    this.modal.close({
      Turma: model,
      ImportAlunos: model.importarAlunos
    });
  }
}
