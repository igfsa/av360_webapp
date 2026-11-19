import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { mergeApplicationConfig } from '@angular/core';
import { App } from '@app/app';
import { serverConfig } from '@core/config/app.config.server';
import { appConfig } from '@core/config/app.config';


const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(
    App,
    mergeApplicationConfig(appConfig, serverConfig),
    context
  );

export default bootstrap;
