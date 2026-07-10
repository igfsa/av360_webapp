import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { form, FormField, maxLength, minLength, pattern, required, validate } from '@angular/forms/signals';

import { DynamicDialogRef } from 'primeng/dynamicdialog';

import { Professor } from '../../../Models/Professor';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlertService } from '../../shared/alert/alert.service';


@Component({
  selector: 'app-professor-criar-modal',
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
        <input type="text" class="form-control" [formField]="professorForm.userName" aria-label="UserName" >
        <small [class.text-danger]="professorForm.userName().value().length >= 50">
          {{ professorForm.userName().value().length }}/50
        </small>
        @if (professorForm.userName().touched() && professorForm.userName().invalid()){
          <ul class="error-list">
            @for (error of professorForm.userName().errors(); track error) {
              <li>{{ error.message }}</li>
            }
          </ul>
        }

        <label>Senha:</label>
        <input [type]="ocultarSenha() ? 'password' : 'text'"
          class="form-control"
          [formField]="professorForm.senhaHash"
          aria-label="Senha" >
        <label style="display: inline-block">
          <input type="checkbox"
            class="form-check-input"
            [checked]="!ocultarSenha()"
            (click)="ocultarSenha.update(v => !v)">
          Mostrar Senha
        </label>

        @if (professorForm.senhaHash().touched() && professorForm.senhaHash().invalid()){
          <ul class="error-list">
            @for (error of professorForm.senhaHash().errors(); track error) {
              <li>{{ error.message }}</li>
            }
          </ul>
        }

        <label>Confirmar Senha:</label>
        <input [type]="ocultarSenha() ? 'password' : 'text'"
          class="form-control"
          [formField]="professorForm.confirmarSenha"
          aria-label="ConfirmarSenha" >
        @if (professorForm.confirmarSenha().touched() && professorForm.confirmarSenha().invalid()){
          <ul class="error-list">
            @for (error of professorForm.confirmarSenha().errors(); track error) {
              <li>{{ error.message }}</li>
            }
          </ul>
        }

     </div>
  </app-modal-layout>
  `
})
export class ProfessorCriarModalComponent implements OnInit {

  professorModel = signal<Professor & {confirmarSenha: string}>({
    id: 0,
    nome: ``,
    userName: ``,
    senhaHash: ``,
    confirmarSenha: ``
  })

  ocultarSenha = signal<boolean>(true);

  professorForm = form(this.professorModel, schemaPath => {
    required(schemaPath.nome, {message: `Nome do Professor deve ser inserido`});
    maxLength(schemaPath.nome, 100, {message: 'Nome do Professor não pode ter mais de 100 caracteres.'});

    required(schemaPath.userName, {message: `Usuário deve ser inserido`});
    maxLength(schemaPath.userName, 100, {message: 'Usuário não pode ter mais de 100 caracteres.'});
    pattern(schemaPath.userName, /^[a-zA-Z0-9]+$/, {message: `Usuário deve conter apenas letras e números.`});

    required(schemaPath.senhaHash, {message: `Senha deve ser inserida`});
    maxLength(schemaPath.senhaHash, 50, {message: 'Senha não pode ter mais de 50 caracteres.'});
    minLength(schemaPath.senhaHash, 8, {message: 'Senha não pode ter menos de 8 caracteres.'});
    pattern(schemaPath.senhaHash, /[A-Z]/, {message: `Senha deve conter pelo menos uma letra maiúscula.`});
    pattern(schemaPath.senhaHash, /[a-z]/, {message: `Senha deve conter pelo menos uma letra minúscula.`});
    pattern(schemaPath.senhaHash, /\d/, {message: `Senha deve conter pelo menos um número.`});
    pattern(schemaPath.senhaHash, /[^A-Za-z\d]/, {message: `Senha deve conter pelo menos um caractere especial.`});

    required(schemaPath.confirmarSenha, {message: `Senha deve ser confirmada`});
    validate(schemaPath.confirmarSenha, ({value, valueOf}) => {
      const confirmPassword = value();
      const password = valueOf(schemaPath.senhaHash);
      if (confirmPassword !== password) {
        return {
          kind: 'passwordMismatch',
          message: 'Confirmação de senha inválida.',
        };
      }
      return null;
    });
  })

  constructor(
    public ref: DynamicDialogRef,
    private formHelper: FormsHelper,
    private alert: AlertService
  ) {}

  ngOnInit(): void {
  }

  public confirmar(): void {
    this.formHelper.markAllTouched(this.professorForm);

    if (this.professorForm().invalid())
    {
      this.alert.toastError(`Verifique os dados do Professor`);
      return
    }

    const { confirmarSenha, ...professor } = this.professorModel();

    this.ref.close(professor)
  }
}
