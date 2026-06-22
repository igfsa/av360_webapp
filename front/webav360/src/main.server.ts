import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { mergeApplicationConfig } from '@angular/core';
import { App } from './app/app';
import { serverConfig } from './app/app.config.server';
import { appConfig } from './app/app.config';


const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(
    App,
    mergeApplicationConfig(appConfig, serverConfig),
    context
  );

export default bootstrap;
