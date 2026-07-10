import { Component, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, min, max, required, maxLength } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';

import { DynamicDialogRef } from 'primeng/dynamicdialog';

import { FormsHelper } from '../../../Helpers/formsHelper';
import { TurmaCriarModalData } from '../../../Models/ModalData';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlertService } from '../../shared/alert/alert.service';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
    ModalLayoutComponent
],
  template: `
  <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Nome da Turma: </label>
      <input type="text" class="form-control" [formField]="turmaForm.turma.cod" aria-label="Cod" >
      <small [class.text-danger]="turmaForm.turma.cod().value().length >= 100">
        {{ turmaForm.turma.cod().value().length }}/100
      </small>
      @if (turmaForm.turma.cod().touched() && turmaForm.turma.cod().invalid()) {
        <ul class="error-list">
          @for (error of turmaForm.turma.cod().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
      <label>Nota Máxima: </label>
      <input type="number" class="form-control" [formField]="turmaForm.turma.notaMax" aria-label="Nota Máxima" >
      @if (turmaForm.turma.notaMax().touched() && turmaForm.turma.notaMax().invalid()){
        <ul class="error-list">
          @for (error of turmaForm.turma.notaMax().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
      <label style="display: inline-block">
        <input type="checkbox" class="form-check-input" [formField]="turmaForm.importarAlunos" aria-label="Importar Alunos">
        Importar alunos após salvar
      </label>
    </div>
  </app-modal-layout>
  `
})
export class TurmaCriarModalComponent implements OnInit {

  turmaModel = signal<TurmaCriarModalData>({
    turma: {
      id: 0,
      cod: '',
      notaMax: 0
    },
    importarAlunos: true
  })

  turmaForm = form(this.turmaModel, schemaPath => {
    required(schemaPath.turma.cod, {message: `Nome da Turma deve ser inserido`})
    maxLength(schemaPath.turma.cod, 100, {message: 'Nome da Turma não pode ter mais de 100 caracteres.'});

    required(schemaPath.turma.notaMax, {message: `Nota Máxima da turma deve ser inserida`});
    min(schemaPath.turma.notaMax, 1, {message: `Nota Máxima não pode ser menor que 1`});
    max(schemaPath.turma.notaMax, 100, {message: `Nota Máxima não pode ser maior que 100`});
  });

  constructor(
    public ref: DynamicDialogRef,
    private formHelper: FormsHelper,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
  }

  public confirmar(): void {
    this.formHelper.markAllTouched(this.turmaForm);

    if (this.turmaForm().invalid())
    {
      this.alert.toastError(`Verifique os dados da Turma`);
      return
    }

    this.ref.close(this.turmaModel());
  }
}
