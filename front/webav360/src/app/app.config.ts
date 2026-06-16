import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideClientHydration } from '@angular/platform-browser';

import { providePrimeNG } from 'primeng/config';
import { definePreset } from '@primeuix/themes';
import Material from '@primeuix/themes/material';

import { AuthInterceptor } from './auth/auth.interceptor';
import { routes } from './app.routes';

const AV360Theme = definePreset(Material, {
    semantic: {
        primary: '{violet}'
    }
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withFetch(), withInterceptors([AuthInterceptor])),
    provideClientHydration(),
    providePrimeNG({
      theme: {
        preset: AV360Theme,
        options: {
          darkModeSelector: false || 'none'
        }
      }
    })
  ]
};
