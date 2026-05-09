import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { form, FormField, required } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';

import { Turma } from '../../../Models/Turma';
import { ImportAlunos } from '../../../Models/TurmaImport';
import { FormsHelper } from '../../../Helpers/formsHelper';

@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
  <div class="modal-header">
    <h1 class="modal-title">Importar Alunos</h1>
  </div>

  <form (ngSubmit)="salvar()" class="d-flex flex-column vh-100">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Nome do Aluno: </label>
      <input type="text" class="form-control" [formField]="importForm.colunaNome" aria-label="colunaNome">
      @if (importForm.colunaNome().touched() && importForm.colunaNome().invalid()) {
        <ul class="error-list">
          @for (error of importForm.colunaNome().errors(); track error) {
            <li>{{ error.message }}</li>
          }
        </ul>
      }

      <label>Arquivo .csv: </label>
      <input type="file" accept=".csv" class="form-control" (change)="onFile($event)">
      @if (importForm.colunaNome().touched() && importForm.file().invalid()) {
        <ul class="error-list">
          @for (error of importForm.file().errors(); track error) {
            <li class="text-danger fw-bold">{{ error.message }}</li>
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
export class TurmaImportModalComponent implements OnInit {
  @Input() turma!: Turma

  importModel = signal<ImportAlunos>({
    colunaNome: '',
    turmaId: 0,
    file: null
  })

  importForm = form(this.importModel, (schemaPath) => {
      required(schemaPath.colunaNome, {message: `Coluna Nome do Aluno deve ser preenchida.`});
      required(schemaPath.file, {message: `Arquivo deve ser inserido.`});
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

  public onFile(event: any) {
    const file = event.target.files[0];

    this.importModel.update(model => ({
      ...model,
      file
    }));

    this.importForm.file().markAsTouched();
  }

  public salvar(): void {
    this.formHelper.markAllTouched(this.importForm);

    if (this.importForm().invalid())
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
        text: `Verifique os dados de Importação`
      });
      return
    }

    this.importModel.update(model => ({
      ...model,
      turmaId: this.turma.id
    }));

    this.modal.close(this.importModel())
  }
}
