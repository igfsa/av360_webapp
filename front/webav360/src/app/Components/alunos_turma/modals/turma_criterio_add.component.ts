import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

import { Turma } from '../../../Models/Turma';
import { Criterio } from '../../../Models/Criterio';
import { CriterioCheckbox } from '../../../Models/CriterioCheckbox';
import { TurmaCriterioModalData } from '../../../Models/ModalData';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { TableModule } from "primeng/table";

@Component({
  selector: 'app-turma-criterio-add-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ModalLayoutComponent,
    TableModule
],
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
      [value]="criteriosCheck"
      dataKey="id"
      [paginator]=true
      [rows]=50
      [globalFilterFields]="['nome']"
      class="table"
      >

      <ng-template #header>
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
      </ng-template>

      <ng-template #body let-criterio>
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
      </ng-template>

      <ng-template #emptymessage>
        <tr>
          <td colspan="8" class="text-center">
            <h4>Nenhum Critério encontrado!</h4>
          </td>
        </tr>
      </ng-template>
    </p-table>

  </app-modal-layout>
  `
})
export class TurmaCriterioModalComponent implements OnInit {

  turma!: Turma;
  criteriosTurma!: Criterio[];
  criterios!: Criterio[]

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
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<TurmaCriterioModalData>,
  ) {}

  private get data(): TurmaCriterioModalData {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.turma = this.data.turma;
    this.criterios = this.data.criterios;
    this.criteriosTurma = this.data.criteriosTurma;

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

  public confirmar(): void {
    const criteriosSelecionados = this.criteriosCheck
      .filter(c => c.selecionado)
      .map(c => c.id);

      this.ref.close(criteriosSelecionados);
  }
}


