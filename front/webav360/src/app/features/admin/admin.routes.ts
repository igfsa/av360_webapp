import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  {
    path: 'professores',
    loadComponent: () => import('./professores/professores.component')
      .then(m => m.ProfessoresComponent)
  },
  {
    path: 'turmas',
    loadComponent: () => import('./turmas_adm/turmas_adm.component')
      .then(m => m.TurmasAdmComponent)
  },
  // {
  //   path: 'criterios',
  //   loadComponent: () => import('./criterios-admin/criterios-admin.component')
  //     .then(m => m.CriteriosAdminComponent)
  // },
  { path: '', redirectTo: '**', pathMatch: 'full' },
];
