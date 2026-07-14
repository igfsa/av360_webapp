const isBrowser = typeof window !== 'undefined';

export const environment = {
  production: true,
  apiUrl: isBrowser ? (window as any).env?.API_URL : 'https://webav-360.fly.dev',
  frontUrl: isBrowser ? (window as any).env?.FRONT_URL : 'https://webav360.riss.com.br'
};
