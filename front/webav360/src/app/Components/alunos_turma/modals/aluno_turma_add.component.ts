import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, maxLength, required } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';

import { DynamicDialogRef } from 'primeng/dynamicdialog';

import { Aluno } from '../../../Models/Aluno';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlertService } from '../../shared/alert/alert.service';

@Component({
  selector: 'app-aluno-turma-add-modal',
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
    <div class="flex-grow-1 overflow-auto" >
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
  </app-modal-layout>
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

  constructor(
    public ref: DynamicDialogRef,
    private formHelper: FormsHelper,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
  }

  public confirmar(): void {
    this.formHelper.markAllTouched(this.alunoForm);

    if (this.alunoForm().invalid())
    {
      this.alert.toastError(`Verifique as informações do Aluno`);
      return
    }

    this.ref.close(this.alunoModel());
  }
}

