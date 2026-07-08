import { ChangeDetectionStrategy, Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { form, FormField, required } from '@angular/forms/signals';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import * as jschardet from 'jschardet';
import Papa from 'papaparse';

import { Turma } from '../../../Models/Turma';
import { ImportAlunos } from '../../../Models/TurmaImport';
import { FormsHelper } from '../../../Helpers/formsHelper';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    FormField,
    ModalLayoutComponent
],
  styleUrls: ['../turmas.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
  <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">
    <div class="modal-body flex-grow-1 overflow-auto" >
      <label>Arquivo .csv: </label>
      <input type="file" accept=".csv" class="form-control" (change)="onFile($event)">
      @if (importForm.colunaNome().touched() && importForm.file().invalid()) {
        <ul class="error-list">
          @for (error of importForm.file().errors(); track error) {
            <li class="text-danger fw-bold">{{ error.message }}</li>
          }
        </ul>
      }

      <label>Selecione a coluna com nome do aluno: </label>
      <select
        class="form-control"
        [formField]="importForm.colunaNome">

        <option value="">
          Selecione a coluna
        </option>

        @for (header of headers(); track header) {

          <option [value]="header">
            {{ header }}
          </option>
        }
      </select>

      @if (previewRows().length > 0) {
        <div class="tabela-wrapper">
          <table class="tabela-csv table text-center table-hover">
            <thead>
              <tr>
                @for (header of headers(); track header) {
                  <th>{{ header }}</th>
                }
              </tr>
            </thead>
            <tbody>
              @for (row of previewRows(); track $index) {
                <tr>
                  @for (header of headers(); track header) {
                    <td>
                      {{ row[header] }}
                    </td>
                  }
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </div>
  </app-modal-layout>
  `
})
export class TurmaImportModalComponent implements OnInit {
  turma!: Turma;

  importModel = signal<ImportAlunos>({
    colunaNome: '',
    turmaId: 0,
    file: null
  })

  importForm = form(this.importModel, (schemaPath) => {
      required(schemaPath.colunaNome, {message: `Coluna Nome do Aluno deve ser preenchida.`});
      required(schemaPath.file, {message: `Arquivo deve ser inserido.`});
  });

  headers = signal<string[]>([]);

  previewRows = signal<any[]>([]);

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<Turma>,
    private formHelper: FormsHelper) {}

  private get data(): Turma {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.turma = this.data;
  }

  public onFile(event: any): void {

    const file = event.target.files[0];

    if (!file)
      return;

    this.importModel.update(model => ({
      ...model,
      file
    }));

    const reader = new FileReader();

    reader.onload = (e: any) => {

      const buffer = e.target.result;

      const uint8Array = new Uint8Array(buffer);

      // texto temporário para detecção
      const sample = new TextDecoder('latin1')
        .decode(uint8Array.slice(0, 10000));

      const detected = jschardet.detect(sample);

      let encoding = detected.encoding?.toLowerCase() ?? 'utf-8';

      // normalização
      if (
        encoding.includes('1252') ||
        encoding.includes('8859') ||
        encoding.includes('latin')
      ) {
        encoding = 'windows-1252';
      }
      else {
        encoding = 'utf-8';
      }

      const csv = new TextDecoder(encoding)
        .decode(uint8Array);

      Papa.parse(csv, {
        header: true,
        skipEmptyLines: true,
        delimiter: ';',

        complete: (result) => {

          const data = result.data as any[];

          if (!data.length)
            return;

          this.headers.set(
            Object.keys(data[0])
          );

          this.previewRows.set(
            data.slice(0, 5)
          );

          // auto detectar coluna nome
          const nomeColumn = this.headers()
            .find(h =>
              h.toLowerCase().includes('nome')
            );

          if (nomeColumn) {

            this.importModel.update(model => ({
              ...model,
              colunaNome: nomeColumn
            }));
          }
        },
        error: () => {

          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: 'Erro ao ler arquivo CSV'
          });
        }
      });
    };

    reader.readAsArrayBuffer(file);

    this.importForm.file().markAsTouched();
  }

  public confirmar(): void {
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

    this.ref.close(this.importModel())
  }
}
