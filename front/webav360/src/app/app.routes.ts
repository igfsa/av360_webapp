import { Routes } from '@angular/router';

import { TurmasComponent } from './Components/turmas/turmas.component';
import { AlunosComponent } from './Components/alunos/alunos.component';
import { HomeComponent } from './Components/home/home.component';
import { CriteriosComponent } from './Components/criterios/criterios.component';
import { AlunoTurmaComponent } from './Components/alunos_turma/alunos_turma.component';

export const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'Turmas', component: TurmasComponent},
  {path: 'AlunosTurma/:id', component: AlunoTurmaComponent},
  {path: 'Alunos', component: AlunosComponent},
  {path: 'Criterios', component: CriteriosComponent}
];
