import { mergeApplicationConfig, ApplicationConfig } from '@angular/core';
import { provideServerRendering } from '@angular/platform-server';
import { appConfig } from './app.config';
import { BrowserstorageserverService } from './Services/BrowserStorageServer/browserstorageserver.service';
import { BrowserStorageService } from './Services/BrowserStorage/browserstorage.service';
import { HttpBackend, HttpXhrBackend } from '@angular/common/http';

const serverConfig: ApplicationConfig = {
  providers: [
    provideServerRendering(),
    {provide:BrowserStorageService, useClass:BrowserstorageserverService}
  ]
};

export const config = mergeApplicationConfig(appConfig, serverConfig);
