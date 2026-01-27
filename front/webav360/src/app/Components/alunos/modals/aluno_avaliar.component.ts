import { ChangeDetectorRef, Component, inject, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { createEmptyTurma, Turma } from '../../../Models/Turma';
import { AvaliarConfirmarModalComponent } from './aluno_avaliar_confirmar.component';
import { Criterio } from '../../../Models/Criterio';
import { CriterioService } from '../../../Service/Criterio.service';


@Component({
  selector: 'app-turma-criar-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
  <div class="modal-header">
    <h4 class="modal-title" style = "font-size: 2.4rem;">Avaliar Aluno</h4>
  </div>

  <div class="modal-body">
    @for (criterio of criterios; track criterio; let c = $index){
      <div class="input-group mb-3 row" >
        <span class="input-group-text col-6" id="basic-addon1" style = "font-size: 1.6rem;">{{criterio.nome}}: </span>
        <input type="decimal" class="form-control" aria-label="Nota" aria-describedby="basic-addon1" style = "font-size: 1.6rem;">
      </div>
    }
  </div>

  <div class="modal-footer">
    <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
    <button class="btn btn-secondary btn-success" (click)="salvar()">Próximo</button>
  </div>
  `
})
export class AvaliarModalComponent implements OnInit {
  private modalService = inject(NgbModal);
  @Input() criterios!: Criterio[];

  constructor(public modal: NgbActiveModal) {}

  ngOnInit(): void {
  }

  salvar(): void {
      const ref = this.modalService.open(AvaliarConfirmarModalComponent, {
        backdrop: 'static',
        centered: true,
        fullscreen: true
      });
      ref.componentInstance.criterios = this.criterios;
      this.modal.close()
  }
}
