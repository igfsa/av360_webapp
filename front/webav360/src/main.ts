/// <reference types="@angular/localize" />

import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from '@core/config/app.config';
import { App } from '@app/app';

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
