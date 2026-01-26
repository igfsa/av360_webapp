import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { createEmptyTurma, Turma } from '../../../Models/Turma';
import { createEmptyImport, ImportAlunos } from '../../../Models/TurmaImport';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
  <div class="modal-header">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Importar Alunos</h4>
  </div>

  <div class="modal-body">
    <div class="input-group m-3 row">
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Código: </span>
      <input type="text" class="form-control" [(ngModel)]="import.colunaNome" aria-label="Cod" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
    </div>
    <div class="input-group m-3 row">
      <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Arquivo .csv: </span>
      <input type="file" accept=".csv" class="form-control" (change)="onFile($event)" style = "font-size: 1.6rem;">
    </div>
  </div>

  <div class="modal-footer">
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()">Salvar</button>
  </div>
  `
})
export class TurmaImportModalComponent implements OnInit {
  @Input() turma!: Turma

  import!: ImportAlunos;

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
    this.import = createEmptyImport();
  }

  public onFile(event: any) {
    this.import.file = event.target.files[0];
  }

  public salvar(): void {
    this.import.turmaId = this.turma.id
    this.modal.close(this.import)
  }
}
