import { Routes } from '@angular/router';

import { TurmasComponent } from './Components/turmas/turmas.component';
import { CriteriosComponent } from './Components/criterios/criterios.component';
import { AlunoTurmaComponent } from './Components/alunos_turma/alunos_turma.component';
import { SessaoComponent } from './Components/sessao/sessao.component';
import { AvaliacaoPublicaComponent } from './Components/avaliacao_publica/avaliacao_publica.component';
import { AvaliacaoEncerradaComponent } from './Components/avaliacao_publica/Aux/avaliacao_encerrada.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './Components/login/login.component';

export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'avaliacao/publica/:token', component: AvaliacaoPublicaComponent},
  {path: 'avaliacao/encerrada', component: AvaliacaoEncerradaComponent},

  {path: '',
    canActivate: [AuthGuard],
    children: [
      {path: 'turmas', component: TurmasComponent},
      {path: 'alunos-turma/:id', component: AlunoTurmaComponent},
      {path: 'criterios', component: CriteriosComponent},
      {path: 'sessao/:id', component: SessaoComponent},
      {path: '', redirectTo: 'turmas', pathMatch: 'full' },
    ]},

  {path: '**', redirectTo: ''},
];
