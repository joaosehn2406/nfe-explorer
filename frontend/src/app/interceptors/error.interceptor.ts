import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { ApiErrorResponse } from '../models/response/api.error.response';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const apiError = extractApiError(error);
      return throwError(() => apiError);
    })
  );
};

function extractApiError(error: HttpErrorResponse): ApiErrorResponse {
  if (error.error instanceof ProgressEvent) {
    return { code: 0, message: 'Sem conexão com o servidor. Verifique sua rede.' };
  }

  if (error.error && typeof error.error === 'object' && 'message' in error.error) {
    return error.error as ApiErrorResponse;
  }

  return { code: error.status, message: `Erro ${error.status}: ${error.statusText || 'Erro inesperado.'}` };
}
