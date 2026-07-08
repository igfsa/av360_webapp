import { Component, OnInit, Inject, } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormArray, FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import Swal from 'sweetalert2';

import { Turma } from '../../../Models/Turma';
import { Grupo } from '../../../Models/Grupo';
import { TurmaGrupoModalData } from '../../../Models/ModalData';
import { ModalLayoutComponent } from "../../shared/modal/modal.component";

@Component({
  selector: 'app-turma-grupo-add-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalLayoutComponent
],
  template: `
  <app-modal-layout
    (cancelar) = "ref.close()"
    (confirmar) = "confirmar()">
    <form [formGroup]="form">
      <div class="flex-grow-1 overflow-auto" formArrayName="grupos">
        @for (grupo of grupos.controls; track grupo.get('id')?.value; let i = $index)
        {
          <div [formGroupName]="i">
            @if(grupo.get('id')?.value === 0){
              <label class="text-success fw-bold">Nova Equipe: </label>
            } @else {
              <label>Equipe: </label>
            }
            <input
              type="text"
              class="form-control"
              formControlName="nome"
              placeholder="Nome da equipe"
            />
            <small [class.text-danger]="grupo.get('nome')?.value.length >= 100">
              {{ grupo.get('nome')?.value.length }}/100
            </small>
            @if (grupo.get('nome')?.touched && grupo.get('nome')?.invalid) {
              <ul class="error-list">
                  <li class="text-danger fw-bold">Nome da equipe deve ser inserido</li>
              </ul>
            }
          </div>
        }

        <button class="btn btn-secondary btn-success"
            type="button"
            (click)="addGrupo()">
          Nova Equipe
        </button>
      </div>
    </form>
  </app-modal-layout>
  `
})
export class TurmaGrupoModalComponent implements OnInit {

  turma!: Turma;
  gruposOrig: Grupo[] = [];
  @Inject(FormBuilder) private fb: FormBuilder = new FormBuilder;

  public gruposEdit: Grupo[] = [];

  form = this.fb.group({
    grupos: this.fb.array([])
  });

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig<TurmaGrupoModalData>,
  ) {}

  private get data(): TurmaGrupoModalData {
    return this.config.data!;
  }

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

    this.turma = this.data.turma;
    this.gruposOrig = this.data.gruposOrig;

    this.gruposOrig.forEach(g =>
      this.grupos.push(this.criaGrupo({nome: g.nome, id: g.id, turmaId: g.turmaId}))
    )
  }

  public addGrupo() {
    if (this.form.invalid){
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
        text: `Todas equipes anteriores devem estar com nome para criar uma nova. Verifique novamente.`
      });
      return
    }

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

  public confirmar(): void {
    this.form.markAllAsTouched();

    if (!this.podeSalvar){
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
        text: `Apenas a última nova equipe pode estar em branco.`
      });
      return
    }

    this.removerUltimoVazio();

    const differences = this.getDiff(this.gruposOrig, this.grupos.value as Grupo[]);

    this.ref.close({edit: differences.edit, add: differences.add});
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
