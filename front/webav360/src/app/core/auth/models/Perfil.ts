export const PERFIS = ['Admin', 'Professor'] as const;

export type Perfil = typeof PERFIS[number];
