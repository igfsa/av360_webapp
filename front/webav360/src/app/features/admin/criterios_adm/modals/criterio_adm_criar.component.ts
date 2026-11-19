import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { form, FormField, maxLength, required } from '@angular/forms/signals';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Criterio } from '@shared/models/Criterio';
import { FormsHelper } from '@shared/helpers/formsHelper';
import { ModalLayoutComponent } from "@shared/components/modal/modal.component";
import { AlertService } from '@shared/components/alert/alert.service';
import { TableModule } from 'primeng/table';


@Component({
  selector: 'app-criterio-adm-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
    ModalLayoutComponent,
    TableModule
],
  template: `
  <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">
    <div class="flex-grow-1 overflow-auto" >
      <p class="text-primary">
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-exclamation-circle-fill" viewBox="0 0 16 16">
          <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4m.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2"/>
        </svg>
         Atenção! O critério será padrão para o sistema. Será incluído para todos professores.
      </p>
      <label>Nome: </label>
      <input type="text" class="form-control" [formField]="criterioForm.nome" aria-label="Nome" >
      <small [class.text-danger]="criterioForm.nome().value().length >= 100">
        {{ criterioForm.nome().value().length }}/100
      </small>

      @if (criterioForm.nome().touched() && criterioForm.nome().invalid()){
        <ul class="error-list">
          @for (error of criterioForm.nome().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }
    </div>
  </app-modal-layout>
  `
})
export class CriterioAdmCriarModalComponent implements OnInit {

  sugestoes: string[] = [];

  criterioModel = signal<Criterio>({
    id: 0,
    nome: '',
    nativo: true
  })

  criterioForm = form(this.criterioModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Critério deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Critério não pode ter mais de 100 caracteres.'});
  })

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<string[]>,
    private formHelper: FormsHelper,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
  }

  public confirmar(): void {
    this.formHelper.markAllTouched(this.criterioForm);

    if (this.criterioForm().invalid())
    {
      this.alert.toastError(`Verifique os dados do Critério`);
      return
    }

    this.ref.close(this.criterioModel())
  }
}
