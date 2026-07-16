import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ScrollPanelModule } from 'primeng/scrollpanel';

import { ModalLayoutComponent } from "../../shared/modal/modal.component";
import { AlunoDashboard } from '../../../Models/Dashboard/AlunoDashboard';

@Component({
  selector: 'app-comentario-acessar-modal',
  standalone: true,
  imports: [
    CommonModule,
    ModalLayoutComponent,
    ScrollPanelModule
],
  template: `
  <app-modal-layout
    [showCancel]="false"
    (confirmar) = "ref.close()"
    confirmarTexto="Ok">
    <div class="flex-grow-1 overflow-auto" >
      <h2>{{aluno.nome}}</h2>
        <p-scrollpanel [style]="{ width: '100%', height: '100%' }">
          <p style="white-space: pre-line;">{{aluno.comentario}}</p>
        </p-scrollpanel>
    </div>
  </app-modal-layout>
  `
})
export class ComentarioAcessarModalComponent implements OnInit {

  aluno!: AlunoDashboard;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<AlunoDashboard>,
  ) {}

  private get data(): AlunoDashboard {
    return this.config.data!;
  }

  ngOnInit(): void {
    this.aluno = this.data;
  }

}
