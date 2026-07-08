import { ApplicationConfig, FactoryProvider, InjectionToken, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideClientHydration } from '@angular/platform-browser';

import { providePrimeNG } from 'primeng/config';
import { definePreset } from '@primeuix/themes';
import Material from '@primeuix/themes/material';

import { AuthInterceptor } from './auth/auth.interceptor';
import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { DialogService } from 'primeng/dynamicdialog';

const AV360Theme = definePreset(Material, {
    semantic: {
        primary: '{violet}'
    }
});

export const API_URL = new InjectionToken<string>('API_URL');
export const FRONT_URL = new InjectionToken<string>('FRONT_URL')

export const apiURLProvider: FactoryProvider = {
  provide: API_URL,
  useFactory: () => environment.apiUrl
};

export const frontURLProvider: FactoryProvider = {
  provide: FRONT_URL,
  useFactory: () => environment.frontUrl
};

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withFetch(), withInterceptors([
      AuthInterceptor
    ])),
    provideClientHydration(),
    providePrimeNG({
      theme: {
        preset: AV360Theme,
        options: {
          darkModeSelector: false || 'none'
        }
      }
    }),
    apiURLProvider,
    frontURLProvider,
    DialogService
  ]
};
