import { Component, input, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../../Models/Turma';
import { Grupo } from '../../../Models/Grupo';
import { AlunoGrupoCheckbox } from '../../../Models/AlunoGrupoCheckbox';
import { AlunoGrupo } from '../../../Models/AlunoGrupo';

@Component({
  selector: 'app-aluno-grupo-add-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-header">
      <h4 class="modal-title" style = "font-size: 2.4rem;">Grupo {{ grupo.nome }}</h4>
    </div>
    <div class="modal-body">
      <table id="main-table" class="table text-center table-hover">
        <thead class="table-dark">
          <tr>
            <th></th>
            <th>
              Aluno
            </th>
            <th>
              Grupo Atual
            </th>
          </tr>
        </thead>
        <tbody>
          @for (aluno of alunosCheck; track aluno; let c = $index){
            <tr (click)="!aluno.desabilitado && toggleAluno(aluno)"
                [class.table-secondary]="aluno.desabilitado"
                style="cursor: pointer"
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
          }
        </tbody>
      </table>
    </div>
    <div class="modal-footer">
      <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
      <button class="btn btn-secondary btn-success" (click)="salvar()">Salvar</button>
    </div>
  `
})
export class AlunoGrupoModalComponent implements OnInit {

  @Input() turma!: Turma;
  @Input() grupo!: Grupo;
  @Input() gruposTurma: Grupo[] = [];
  @Input() alunosCheck: AlunoGrupoCheckbox[] = [];

  constructor(
    public modal: NgbActiveModal,
  ) {}

  ngOnInit(): void {

  }

  public salvar(): void {
    const alunosRes: AlunoGrupo = {
      grupoId: this.grupo.id,
      turmaId: this.turma.id,
      alunoIds: (this.alunosCheck.filter(c => c.selecionado && !c.desabilitado).map(c => c.alunoId))
    }
      this.modal.close(alunosRes);
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


