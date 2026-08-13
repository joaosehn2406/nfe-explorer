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
    return { code: 0, message: 'No connection to the server. Check your network.' };
  }

  if (error.error && typeof error.error === 'object' && 'message' in error.error) {
    return error.error as ApiErrorResponse;
  }

  return { code: error.status, message: `Error ${error.status}: ${error.statusText || 'Unexpected error.'}` };
}
