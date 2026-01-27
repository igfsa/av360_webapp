import { Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { createEmptyTurma, Turma } from '../../../Models/Turma';
import { CriterioService } from '../../../Service/Criterio.service';
import { Criterio } from '../../../Models/Criterio';
import { AvaliarModalComponent } from './aluno_avaliar.component';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
  <div class="modal-header">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Confirmar Avaliar Aluno</h4>
  </div>

  <div class="modal-body">
    <table id="main-table" class="table table-striped text-center table-hover">
      <thead class="table-dark">
        <tr>
          <th>Nome</th>
            @for (criterio of criterios; track criterio; let c = $index){
              <th>{{criterio.nome}}</th>
            }
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>Ana Carolina Nogueira da Silva</td>
            @for (criterio of criterios; track criterio; let c = $index){
              <td>0</td>
            }
        </tr>
        <tr>
          <td>Bernardo Serranegra Ribeiro Silva</td>
            @for (criterio of criterios; track criterio; let c = $index){
              <td>0</td>
            }
        </tr>
      </tbody>
    </table>
  </div>

  <div class="modal-footer">
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()">Próximo</button>
  </div>
  `
})
export class AvaliarConfirmarModalComponent implements OnInit {
  @Input() criterios!: Criterio[];

  constructor(
    public modal: NgbActiveModal,
  ) {}

  ngOnInit(): void {

  }

  salvar(): void {
    this.modal.close();
  }
}
