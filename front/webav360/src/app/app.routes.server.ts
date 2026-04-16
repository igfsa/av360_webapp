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
    path: 'sessao/:id',
    renderMode: RenderMode.Server
  },
  {
    path: '**',
    renderMode: RenderMode.Server
  }
];
