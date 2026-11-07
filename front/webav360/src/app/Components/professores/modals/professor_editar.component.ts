import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { disabled, form, FormField, max, maxLength, min, required } from '@angular/forms/signals';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Professor } from '../../../Models/Professor';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlertService } from '../../shared/alert/alert.service';
import { PERFIS } from '../../../Models/Perfil';


@Component({
  selector: 'app-professor-editar-modal',
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
      <input type="text" class="form-control" [formField]="professorForm.nome" aria-label="Nome" >
      <small [class.text-danger]="professorForm.nome().value().length >= 100">
        {{ professorForm.nome().value().length }}/100
      </small>

      @if (professorForm.nome().touched() && professorForm.nome().invalid()){
        <ul class="error-list">
          @for (error of professorForm.nome().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }

      <label>Usuário:</label>
      <input type="text" class="form-control" [formField]="professorForm.userName" aria-label="UserName">
      <small class="text-primary">
        Não é possível alterar Nome do Usuário.
      </small>

      <label>Perfil:</label>
      @for (perfil of perfis; track perfil) {
        <input
          type="radio"
          class="radio"
          [formField]="professorForm.perfil"
          [value]="perfil"
        />
        {{ perfil }}
      }
    </div>
  </app-modal-layout>

  `
})
export class ProfessorEditarModalComponent implements OnInit {
  readonly perfis = PERFIS;

  professorModel = signal<Professor>({
    id: 0,
    userName: '',
    nome: '',
    perfil: 'Professor',
  });

  professorForm = form(this.professorModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Professor deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Nome do Professor não pode ter mais de 100 caracteres.'});

    disabled(schemaPath.userName);
  });

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<Professor>,
    private formHelper: FormsHelper,
    private alert: AlertService
  ) {}

  private get data(): Professor {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.professorModel.set({
      ...this.data
    });
  }

  public confirmar(): void {
    this.formHelper.markAllTouched(this.professorForm);

    if (this.professorForm().invalid())
    {
      this.alert.toastError(`Verifique os dados do Professor`);
      return
    }

    this.ref.close(this.professorModel())
  }
}
