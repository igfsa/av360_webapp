import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { form, FormField, maxLength, minLength, pattern, required, validate } from '@angular/forms/signals';

import { Professor } from '../../../Models/Professor';
import { FormsHelper } from '../../../Helpers/formsHelper';


@Component({
  selector: 'app-professor-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
  ],
  template: `
    <div class="modal-header">
      <h1 class="modal-title">Novo Professor</h1>
    </div>

    <form (ngSubmit)="salvar()" class="d-flex flex-column vh-100">
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

      <div class="modal-footer mt-auto">
        <button type="button" class="btn btn-secondary btn-danger" (click)="cancelar($event)">Cancelar</button>
        <button type="submit" class="btn btn-secondary btn-success">Salvar</button>
      </div>
    </form>
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
    this.formHelper.markAllTouched(this.professorForm);

    if (this.professorForm().invalid())
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
        text: `Verifique os dados do Professor`
      });
      return
    }

    const { confirmarSenha, ...professor } = this.professorModel();

    this.modal.close(professor)
  }
}
