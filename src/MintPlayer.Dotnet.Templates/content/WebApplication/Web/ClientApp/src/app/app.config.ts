import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
//#if (EnableXsrf)
import { xsrfInterceptor } from './interceptors/xsrf.interceptor';
//#endif
//#if (EnableConcurrencyHandling)
import { concurrencyInterceptor } from './interceptors/concurrency.interceptor';
//#endif
//#if (UseNgxTranslate)
import { provideTranslateService, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
//#endif

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withFetch(),
      withInterceptors([
//#if (EnableXsrf)
        xsrfInterceptor,
//#endif
//#if (EnableConcurrencyHandling)
        concurrencyInterceptor,
//#endif
      ])
    ),
    provideAnimations(),
//#if (UseNgxTranslate)
    provideTranslateService({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
//#endif
  ]
};
