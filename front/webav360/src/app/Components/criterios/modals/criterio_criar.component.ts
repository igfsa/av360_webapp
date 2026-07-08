import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { form, FormField, maxLength, required } from '@angular/forms/signals';

import { DynamicDialogRef } from 'primeng/dynamicdialog';
import Swal from 'sweetalert2';

import { Criterio } from '../../../Models/Criterio';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";


@Component({
  selector: 'app-criterio-criar-modal',
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
export class CriterioCriarModalComponent implements OnInit {

  criterioModel = signal<Criterio>({
    id: 0,
    nome: ''
  })

  criterioForm = form(this.criterioModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Critério deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Critério não pode ter mais de 100 caracteres.'});
  })

  constructor(
    public ref: DynamicDialogRef,
    private formHelper: FormsHelper
  ) {}

  ngOnInit(): void {
  }

  public confirmar(): void {
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

    this.ref.close(this.criterioModel())
  }
}
