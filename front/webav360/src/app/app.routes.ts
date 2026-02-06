import { Routes } from '@angular/router';

import { TurmasComponent } from './Components/turmas/turmas.component';
import { AlunosComponent } from './Components/alunos/alunos.component';
import { HomeComponent } from './Components/home/home.component';
import { CriteriosComponent } from './Components/criterios/criterios.component';
import { AlunoTurmaComponent } from './Components/alunos_turma/alunos_turma.component';
import { SessaoComponent } from './Components/sessao/sessao.component';
import { AvaliacaoPublicaComponent } from './Components/avaliacao_publica/avaliacao_publica.component';

export const routes: Routes = [
  {path: '', component: TurmasComponent},
  {path: 'Turmas', component: TurmasComponent},
  {path: 'AlunosTurma/:id', component: AlunoTurmaComponent},
  // {path: 'Alunos', component: AlunosComponent},
  {path: 'Criterios', component: CriteriosComponent},
  {path: 'Sessao/:id', component: SessaoComponent},
  {path: 'avaliacao/publica/:token', component: AvaliacaoPublicaComponent},
];
