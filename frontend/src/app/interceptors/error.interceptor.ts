import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ApiErrorResponse } from '../models/response/api.error.response';
import { LanguageService } from '../services/language.service';
import { translate } from '../shared/translations';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const languageService = inject(LanguageService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiError = extractApiError(error, languageService.getLanguage());
      return throwError(() => apiError);
    })
  );
};

function extractApiError(error: HttpErrorResponse, language: string): ApiErrorResponse {
  if (error.error instanceof ProgressEvent) {
    return { code: 0, message: translate('errors.noConnection', language) };
  }

  if (error.error && typeof error.error === 'object' && 'message' in error.error) {
    return error.error as ApiErrorResponse;
  }

  return {
    code: error.status,
    message: `${translate('errors.statusPrefix', language)} ${error.status}: ${error.statusText || translate('errors.unexpected', language)}`
  };
}
