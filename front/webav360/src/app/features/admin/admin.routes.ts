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
  {
    path: 'criterios',
    loadComponent: () => import('./criterios_adm/criterios_adm.component')
      .then(m => m.CriteriosAdmComponent)
  },
  { path: '', redirectTo: '**', pathMatch: 'full' },
];
