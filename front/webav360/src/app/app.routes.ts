import { Routes } from '@angular/router';

import { TurmasComponent } from './Components/turmas/turmas.component';
import { CriteriosComponent } from './Components/criterios/criterios.component';
import { AlunoTurmaComponent } from './Components/alunos_turma/alunos_turma.component';
import { SessaoAtivaComponent } from './Components/sessao_ativa/sessao_ativa.component';
import { AvaliacaoPublicaComponent } from './Components/avaliacao_publica/avaliacao_publica.component';
import { AvaliacaoEncerradaComponent } from './Components/avaliacao_publica/aux/avaliacao_encerrada.component';
import { AuthGuard } from './auth/auth.guard';
import { LoginComponent } from './Components/login/login.component';
import { SessaoQrCodeComponent } from './Components/sessao_ativa/aux/sessao_qrcode.component';
import { SessaoHistComponent } from './Components/sessao_hist/sessao_hist.component';

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
      {path: 'sessao-ativa/:id', component: SessaoAtivaComponent},
      {path: `sessao-qrcode/:id`, component: SessaoQrCodeComponent},
      {path: 'sessao-hist/:id', component: SessaoHistComponent},
      {path: '', redirectTo: 'turmas', pathMatch: 'full' },
    ]},

  {path: '**', redirectTo: ''},
];
