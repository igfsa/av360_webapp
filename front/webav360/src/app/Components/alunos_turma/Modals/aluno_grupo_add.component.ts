import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../../Models/Turma';
import { Criterio } from '../../../Models/Criterio';
import { CriterioCheckbox } from '../../../Models/CriterioCheckbox';

@Component({
  selector: 'app-aluno-grupo-add-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-header">
      <h4 class="modal-title" style = "font-size: 2.4rem;">Turma {{ turma.cod }}</h4>
    </div>
    <div class="modal-body">
      <table class="table table-hover">
        <thead>
          <tr (click)="toggleTodos()"
                style="cursor: pointer">
            <td width="4rem">
              <input type="checkbox" class="form-check-input"
                [checked]="todosSelecionados"
                [indeterminate]="algunsSelecionados"
                (click)="onToggleTodosCheckbox($event)" />
            </td>
            <td>
              {{ todosSelecionados
              ? 'Desmarcar todos'
              : 'Marcar todos' }}
            </td>
          </tr>
        </thead>
        <tbody>
          @for (criterio of criteriosCheck; track criterio; let c = $index){
            <tr (click)="toggleCriterio(criterio)"
                 style="cursor: pointer">
              <td width="4rem">
                <input type="checkbox" class="form-check-input"
                      [(ngModel)]="criterio.selecionado"
                      (click)="$event.stopPropagation()" />
              </td>
              <td>
                {{ criterio.nome }}
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
  @Input() criteriosTurma!: Criterio[];
  @Input() criterios!: Criterio[]

  public criterioId: number[] = [];
  public criteriosCheck: CriterioCheckbox[] = [];

  get todosSelecionados(): boolean {
    return (
      this.criteriosCheck.length > 0 &&
      this.criteriosCheck.every(c => c.selecionado)
    );
  }

  get algunsSelecionados(): boolean {
    const selecionados = this.criteriosCheck.filter(c => c.selecionado).length;
    return selecionados > 0 && selecionados < this.criteriosCheck.length;
  }

  constructor(
    public modal: NgbActiveModal,
  ) {}

  ngOnInit(): void {
    this.getCriterios();
  }

  public getCriterios (): void{
    this.criterios.forEach(c => {
          this.criteriosCheck.push({
                                    id: c.id,
                                    nome: c.nome,
                                    selecionado: this.validaCriterioTurma(c).length != 0
                                  });
        })
      }

  public validaCriterioTurma (criterio: Criterio) : Criterio[]{
    return this.criteriosTurma.filter(c => c.id == criterio.id);
  }

  public toggleCriterio(ct: CriterioCheckbox): void {
    ct.selecionado = !ct.selecionado;
  }

  public toggleTodos(): void {
    if (this.criteriosCheck.filter(c => c.selecionado).length === this.criterios.length) {
      this.criteriosCheck.forEach(c => c.selecionado = false)
    } else {
      this.criteriosCheck.forEach(c => c.selecionado = true)
    }
  }

  public onToggleTodosCheckbox(event: MouseEvent): void {
    event.stopPropagation();
    this.toggleTodos();
  }

  public salvar(): void {
    const criteriosSelecionados = this.criteriosCheck
      .filter(c => c.selecionado)
      .map(c => c.id);

      this.modal.close(criteriosSelecionados);
  }
}


