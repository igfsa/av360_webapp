import { Component, Input, OnInit, ChangeDetectionStrategy, inject, } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { Turma } from '../../../Models/Turma';
import { Grupo } from '../../../Models/Grupo';

@Component({
  selector: 'app-turma-grupo-add-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="modal-header">
      <h4 class="modal-title" style = "font-size: 2.4rem;">Equipes Turma {{ turma.cod }}</h4>
    </div>
    <div class="modal-body">
      <form [formGroup]="form">
        <table class="table table-hover">
          <tbody>
            <div formArrayName="grupos">
              @for (grupo of grupos.controls; track grupo.get('id')?.value; let i = $index)
              {
                <div class="input-group mb-3 row" [formGroupName]="i">
                  <span class="input-group-text col-2" id="basic-addon1" style = "font-size: 1.6rem;">Equipe: </span>
                  <input
                    type="text"
                    class="form-control"
                    style="font-size: 1.6rem;"
                    formControlName="nome"
                    placeholder="Nome da equipe"
                  />
                  @if (grupo.get('nome')?.touched && grupo.get('nome')?.invalid) {
                    <div class="error">
                      <span class="text-danger fw-bold" >Nome inválido...</span>
                    </div>
                  }
                </div>
              }
            </div>
          </tbody>
          <tfoot>
            <button class="btn btn-secondary btn-success"
                type="button"
                (click)="addGrupo()"
                [disabled]="form.invalid">
              Nova Equipe
            </button>
          </tfoot>
        </table>
      </form>
    </div>
    <div class="modal-footer">
      <button class="btn btn-secondary btn-danger" (click)="modal.dismiss()">Cancelar</button>
      <button class="btn btn-secondary btn-success" (click)="salvar()" [disabled]="!podeSalvar">Salvar</button>
    </div>
  `
})
export class TurmaGrupoModalComponent implements OnInit {

  @Input() turma!: Turma;
  @Input() gruposOrig: Grupo[] = [];

  private fb = inject(FormBuilder)
  public gruposEdit: Grupo[] = [];

  form = this.fb.group({
    grupos: this.fb.array([])
  });

  constructor(
    public modal: NgbActiveModal,
  ) {}

  get grupos(): FormArray {
    return this.form.get('grupos') as FormArray;
  }

  get podeSalvar(): boolean {
    if (this.form.valid) return true;

    const total = this.grupos.length;
    if (total === 0) return true;

    const ultimo = this.grupos.at(total - 1) as FormGroup;

    const id = ultimo.get('id')?.value;
    const nome = ultimo.get('nome')?.value;

    const ehNovo = id == null || id === 0;
    const nomeVazio = !nome || nome.trim() === '';

    if (ehNovo && nomeVazio) {
      for (let i = 0; i < total - 1; i++) {
        const grupo = this.grupos.at(i) as FormGroup;

        if (grupo.invalid) {
          return false;
      }}
      return true;
    }

    return false;
  }

  ngOnInit(): void {
    this.grupos.clear();

    this.gruposOrig.forEach(g =>
      this.grupos.push(this.criaGrupo({nome: g.nome, id: g.id, turmaId: g.turmaId}))
    )
  }

  public addGrupo() {
    this.grupos.push(this.criaGrupo());
  }

  public criaGrupo (grupo?: Grupo): FormGroup{
    return this.fb.group({
      id: [grupo?.id ?? 0],
      nome: [grupo?.nome ?? '', Validators.required],
      turmaId: [this.turma.id]
    });
  }

  private removerUltimoVazio(): void {
    const total = this.grupos.length;
    if (total === 0) return;

    const ultimo = this.grupos.at(total - 1) as FormGroup;

    const id = ultimo.get('id')?.value;
    const nome = ultimo.get('nome')?.value;

    const novo = id === 0 || id === null || id === undefined;
    const nomeVazio = !nome || nome.trim() === '';

    if (novo && nomeVazio) {
      this.grupos.removeAt(total - 1);
    }
  }

  public salvar(): void {
    this.removerUltimoVazio();

    const differences = this.getDiff(this.gruposOrig, this.grupos.value as Grupo[]);

    // console.log(differences.edit, differences.add);
    this.modal.close({edit: differences.edit, add: differences.add});
  }

  public getDiff(original: Grupo[], novo: Grupo[]) {
      const add = novo.filter(u => !original.some(o => o.id === u.id));

      const edit = novo.filter(u => {
          const correspOrigin = original.find(o => o.id === u.id);
          return correspOrigin && u.nome !== correspOrigin.nome;
      });

      return {add, edit};
  }


}


