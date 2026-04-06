import { HttpClient, HttpErrorResponse, HttpEventType, HttpRequest } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { ImportNfeResponse } from '../models/response/import.nfe.response';
import { Injectable } from '@angular/core';
import { ApiErrorResponse } from '../models/response/api.error.response';

export interface UploadProgress {
  progress: number;
  result?: ImportNfeResponse;
}

@Injectable({ providedIn: 'root' })
export class NfeService {

  private readonly baseUrl = '/api/nfe_explorer';

  constructor(private http: HttpClient) {
  }

  importNfe(payload: File | string): Observable<UploadProgress> {
    const formData = new FormData();

    if (payload instanceof File) {
      formData.append('File', payload, payload.name);
    } else {
      formData.append('XmlText', payload);
    }

    const req = new HttpRequest('POST', `${this.baseUrl}/import`, formData, {
      reportProgress: true
    });

    return this.http.request(req).pipe(
      map(event => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          return { progress: Math.round((event.loaded / event.total) * 100) };
        }
        if (event.type === HttpEventType.Response) {
          return { progress: 100, result: event.body as ImportNfeResponse };
        }
        return { progress: 0 };
      }),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => error.error as ApiErrorResponse);
  }
}
