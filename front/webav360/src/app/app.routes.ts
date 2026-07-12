import { Routes } from '@angular/router';

import { DialogService, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { AuthGuard } from './auth/auth.guard';
import { ModalService } from './Components/shared/modal/modal.service';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./Components/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'avaliacao/publica/:token',
    loadComponent: () => import('./Components/avaliacao_publica/avaliacao_publica.component').then(m => m.AvaliacaoPublicaComponent)
  },
  {
    path: 'avaliacao/encerrada',
    loadComponent: () => import('./Components/avaliacao_publica/aux/avaliacao_encerrada.component').then(m => m.AvaliacaoEncerradaComponent)
  },

  {
    path: '',
    canActivate: [AuthGuard],
    providers: [DialogService, DynamicDialogConfig, ModalService],
    children: [
      {
        path: 'turmas',
        loadComponent: () => import('./Components/turmas/turmas.component').then(m => m.TurmasComponent)
      },
      {
        path: 'alunos-turma/:id',
        loadComponent: () => import('./Components/alunos_turma/alunos_turma.component').then(m => m.AlunoTurmaComponent)
      },
      {
        path: 'criterios',
        loadComponent: () => import('./Components/criterios/criterios.component').then(m => m.CriteriosComponent)
      },
      {
        path: 'sessao-ativa/:id',
        loadComponent: () => import('./Components/sessao_ativa/sessao_ativa.component').then(m => m.SessaoAtivaComponent)
      },
      {
        path: 'sessao-qrcode/:id',
        loadComponent: () => import('./Components/sessao_ativa/aux/sessao_qrcode.component').then(m => m.SessaoQrCodeComponent)
      },
      {
        path: 'sessao-hist/:id',
        loadComponent: () => import('./Components/sessao_hist/sessao_hist.component').then(m => m.SessaoHistComponent)
      },
      {
        path: 'professores',
        loadComponent: () => import('./Components/professores/professores.component').then(m => m.ProfessoresComponent)
      },
      {
        path: '',
        redirectTo: 'turmas',
        pathMatch: 'full'
      },
    ]
  },

  { path: '**', redirectTo: 'login' },
];
