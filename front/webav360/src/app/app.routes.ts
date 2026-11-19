import { Routes } from '@angular/router';
import { DialogService, DynamicDialogConfig } from 'primeng/dynamicdialog';

import { AuthGuard } from '@core/auth/auth.guard';
import { AdminGuard } from '@core/auth/admin.guard';
import { ModalService } from '@shared/components/modal/modal.service';

// ModalService não tem providedIn, então precisa ser declarado em cada área que abre modais.
const MODAL_PROVIDERS = [DialogService, DynamicDialogConfig, ModalService];

export const routes: Routes = [

  {
    path: 'login',
    loadComponent: () => import('@features/public/login/login.component')
      .then(m => m.LoginComponent)
  },
  {
    path: 'avaliacao/publica/:token',
    loadComponent: () => import('@features/public/avaliacao_publica/avaliacao_publica.component')
      .then(m => m.AvaliacaoPublicaComponent)
  },
  {
    path: 'avaliacao/encerrada',
    loadComponent: () => import('@features/public/avaliacao_publica/helpers/avaliacao_encerrada.component')
      .then(m => m.AvaliacaoEncerradaComponent)
  },

  {
    path: 'admin',
    canActivate: [AuthGuard, AdminGuard],
    providers: MODAL_PROVIDERS,
    loadChildren: () => import('@features/admin/admin.routes')
      .then(m => m.ADMIN_ROUTES)
  },

  {
    path: '',
    canActivate: [AuthGuard],
    providers: MODAL_PROVIDERS,
    loadChildren: () => import('@features/professor/professor.routes')
      .then(m => m.PROFESSOR_ROUTES)
  },

  { path: '**', redirectTo: 'login' },
];
