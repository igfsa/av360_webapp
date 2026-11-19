import { Routes } from '@angular/router';

export const PROFESSOR_ROUTES: Routes = [
  {
    path: 'home',
    loadComponent: () => import('@features/home/home.component')
      .then(m => m.HomeComponent)
  },
  {
    path: 'turmas',
    loadComponent: () => import('@features/professor/turmas/turmas.component')
      .then(m => m.TurmasComponent)
  },
  {
    path: 'alunos-turma/:id',
    loadComponent: () => import('@features/professor/alunos_turma/alunos_turma.component')
      .then(m => m.AlunoTurmaComponent)
  },
  {
    path: 'criterios',
    loadComponent: () => import('@features/professor/criterios/criterios.component')
      .then(m => m.CriteriosComponent)
  },
  {
    path: 'sessao-ativa/:id',
    loadComponent: () => import('@features/professor/sessao_ativa/sessao_ativa.component')
      .then(m => m.SessaoAtivaComponent)
  },
  {
    path: 'sessao-qrcode/:id',
    loadComponent: () => import('@features/professor/sessao_ativa/helpers/sessao_qrcode.component')
      .then(m => m.SessaoQrCodeComponent)
  },
  {
    path: 'sessao-hist/:id',
    loadComponent: () => import('@features/professor/sessao_hist/sessao_hist.component')
      .then(m => m.SessaoHistComponent)
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '**', redirectTo: 'home' },
];
