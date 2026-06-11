import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: 'avaliacao/publica/:token',
    renderMode: RenderMode.Server
  },
  {
    path: 'alunos-turma/:id',
    renderMode: RenderMode.Server
  },
  {
    path: 'sessao-ativa/:id',
    renderMode: RenderMode.Server
  },
  {
    path: 'sessao-hist/:id',
    renderMode: RenderMode.Server
  },
  {
    path: '**',
    renderMode: RenderMode.Server
  }
];
