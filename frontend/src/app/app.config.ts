import {ApplicationConfig, LOCALE_ID, provideBrowserGlobalErrorListeners} from '@angular/core';
import {provideRouter} from '@angular/router';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {registerLocaleData} from '@angular/common';
import localePt from '@angular/common/locales/pt';

import {routes} from './app.routes';
import {errorInterceptor} from './interceptors/error.interceptor';
import {languageInterceptor} from './interceptors/language.interceptor';

registerLocaleData(localePt);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([languageInterceptor, errorInterceptor])),
    provideAnimationsAsync(),
    {provide: LOCALE_ID, useValue: 'pt-BR'}
  ]
};
