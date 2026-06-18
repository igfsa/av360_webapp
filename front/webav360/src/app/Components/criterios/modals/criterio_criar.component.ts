import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { form, FormField, maxLength, required } from '@angular/forms/signals';

import { Criterio } from '../../../Models/Criterio';
import { FormsHelper } from '../../../Helpers/formsHelper';


@Component({
  selector: 'app-criterio-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
  ],
  template: `
    <div class="modal-header">
      <h1 class="modal-title">Novo Critério</h1>
    </div>

    <form (ngSubmit)="salvar()" class="d-flex flex-column vh-100">
      <div class="modal-body flex-grow-1 overflow-auto" >
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

      <div class="modal-footer mt-auto">
        <button type="button" class="btn btn-secondary btn-danger" (click)="cancelar($event)">Cancelar</button>
        <button type="submit" class="btn btn-secondary btn-success">Salvar</button>
      </div>
    </form>
  `
})
export class CriterioCriarModalComponent implements OnInit {

  criterioModel = signal<Criterio>({
    id: 0,
    nome: ''
  })

  criterioForm = form(this.criterioModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Critério deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Critério não pode ter mais de 100 caracteres.'});
  })

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
    this.formHelper.markAllTouched(this.criterioForm);

    if (this.criterioForm().invalid())
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
        text: `Verifique os dados do Critério`
      });
      return
    }

    this.modal.close(this.criterioModel())
  }
}
