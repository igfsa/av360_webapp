import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Turma } from '../../../Models/Turma';
import { Grupo } from '../../../Models/Grupo';
import { AlunoGrupoCheckbox } from '../../../Models/AlunoGrupoCheckbox';
import { AlunoGrupo } from '../../../Models/AlunoGrupo';
import { AlunoGrupoModalData } from '../../../Models/ModalData';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { TableModule } from "primeng/table";

@Component({
  selector: 'app-aluno-grupo-add-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ModalLayoutComponent, TableModule],
  template: `
    <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">

    <div class="d-flex justify-content-end">

      <input
        pInputText
        type="text"
        placeholder="Buscar"
        (input)="table.filterGlobal($event.target.value, 'contains')" />

    </div>

    <p-table
      #table
      [value]="alunosCheck"
      dataKey="id"
      [paginator]=true
      [rows]=50
      [globalFilterFields]="['nome']"
      class="table"
      >

      <ng-template #header>
          <tr>
            <th></th>
            <th>Aluno</th>
            <th>Equipe Atual</th>
          </tr>
      </ng-template>

      <ng-template #body let-aluno>
            <tr (click)="!aluno.desabilitado && toggleAluno(aluno)"
                style="cursor: pointer"
                [class]="aluno.desabilitado ? 'table-gray' : ''"
                >
              <td width="4rem">
                <input type="checkbox" class="form-check-input"
                    [(ngModel)]="aluno.selecionado"
                    [disabled]="aluno.desabilitado"
                    (click)="$event.stopPropagation()"
                    data-bs-toggle="tooltip"
                    data-bs-placement="top"
                    data-bs-title="Aluno adicionado em outro grupo"/>
              </td>
              <td style = 'width: 50%;'>
                {{ aluno.nome }}
              </td>
              <td style = 'width: 40%;'>
                @if(aluno.desabilitado && aluno.grupoAtualId){
                  {{ mostraGrupo(aluno.grupoAtualId) }}
                }
              </td>
            </tr>
      </ng-template>

      <ng-template #emptymessage>
        <tr>
          <td colspan="8" class="text-center">
            <h4>Nenhuma Aluno encontrado!</h4>
          </td>
        </tr>
      </ng-template>
    </p-table>
  </app-modal-layout>
  `
})
export class AlunoGrupoModalComponent implements OnInit {

  turma!: Turma;
  grupo!: Grupo;
  gruposTurma: Grupo[] = [];
  alunosCheck: AlunoGrupoCheckbox[] = [];

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<AlunoGrupoModalData>,
  ) {}

  private get data(): AlunoGrupoModalData {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.turma = this.data.turma;
    this.grupo = this.data.grupo;
    this.gruposTurma = this.data.gruposTurma;
    this.alunosCheck = this.data.alunosCheck;
  }

  public confirmar(): void {
    const alunosRes: AlunoGrupo = {
      grupoId: this.grupo.id,
      turmaId: this.turma.id,
      alunoIds: (this.alunosCheck.filter(c => c.selecionado && !c.desabilitado).map(c => c.alunoId))
    }
      this.ref.close(alunosRes);
  }

  public toggleAluno(aluno: AlunoGrupoCheckbox): void {
    aluno.selecionado = !aluno.selecionado;
  }

  public mostraGrupo(id: number): string {
    var res = this.gruposTurma.find(g => g.id == id)?.nome;
    if (!res){
      res = '';
    }
    return res;
  }
}
