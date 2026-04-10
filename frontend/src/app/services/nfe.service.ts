import {HttpClient, HttpErrorResponse, HttpEventType, HttpRequest} from '@angular/common/http';
import {catchError, filter, map, Observable, throwError} from 'rxjs';
import {ImportNfeResponse} from '../models/response/import.nfe.response';
import {Injectable} from '@angular/core';
import {ApiErrorResponse} from '../models/response/api.error.response';

@Injectable({providedIn: 'root'})
export class NfeService {

  private readonly baseUrl = '/api/nfe_explorer';

  constructor(private http: HttpClient) {
  }

  importNfe(payload: File | string): Observable<ImportNfeResponse> {
    const formData = new FormData();

    if (payload instanceof File) {
      formData.append('File', payload, payload.name);
    } else {
      formData.append('XmlText', payload);
    }

    const req = new HttpRequest('POST', `${this.baseUrl}/import`, formData);

    return this.http.request<ImportNfeResponse>(req).pipe(
      filter(event => event.type === HttpEventType.Response),
      map(event => (event as any).body as ImportNfeResponse),
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    return throwError(() => error.error as ApiErrorResponse);
  }
}
