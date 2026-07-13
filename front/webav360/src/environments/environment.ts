export const environment = {
  production: true,
  apiUrl: (window as any).env?.API_URL || 'NG_APP_API_URL',
  frontUrl: (window as any).env?.FRONT_URL || 'NG_APP_FRONT_URL'
};
