import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { form, FormField, maxLength, required } from '@angular/forms/signals';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Criterio } from '../../../Models/Criterio';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlertService } from '../../shared/alert/alert.service';
import { TableModule } from 'primeng/table';


@Component({
  selector: 'app-criterio-criar-modal',
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

      <p-table
        #table
        [value]="sugestoes"
        [paginator]=true
        [rows]=50
        class="table"
        >

        <ng-template #header>
          <tr>
            <th>Nome</th>
            <th style = 'width: 20%; text-align: center;'>Ações</th>
          </tr>
        </ng-template>

        <ng-template #body let-criterio>
          <tr>
            <td>{{criterio}}</td>
            <td style= 'text-align: center'>
              <button type="button" class="btn-editar btn btn-primary btn-sm mr-2 " (click)="replicarCriterio(criterio)" (mousedown)="$event.preventDefault()">
                <svg xmlns="http://www.w3.org/2000/svg" width="1.6rem" height="1.6rem" fill="currentColor" class="bi bi-copy" viewBox="0 0 16 16">
                  <path fill-rule="evenodd" d="M4 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2zm2-1a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 5a1 1 0 0 0-1 1v8a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1v-1h1v1a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h1v1z"/>
                </svg>
                Replicar
              </button>
            </td>
          </tr>
        </ng-template>

        <ng-template #emptymessage>
          <tr>
            <td colspan="8" class="text-center">
              <h4>Nenhum Critério encontrado!</h4>
            </td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  </app-modal-layout>
  `
})
export class CriterioCriarModalComponent implements OnInit {

  sugestoes: string[] = [];

  criterioModel = signal<Criterio>({
    id: 0,
    nome: '',
    nativo: false
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

  private get data(): string[] {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.sugestoes = this.data;
  }

  public replicarCriterio(criterio: string): void {
    this.criterioModel.update(m => ({ ...m, nome: criterio }));
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
