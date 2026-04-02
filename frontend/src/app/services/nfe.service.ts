import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {ImportNfeResponse} from '../models/response/import.nfe.response';
import {Injectable} from '@angular/core';
import {ApiErrorResponse} from '../models/response/api.error.response';

@Injectable({
  providedIn: 'root',
})
export class NfeService {

  private readonly baseUrl = '/api/nfe_explorer'

  constructor(private http: HttpClient) {
  }

  importByFile(file: File): Observable<ImportNfeResponse> {
    const formData = new FormData()
    formData.append('File', file, file.name)

    return this.http
      .post<ImportNfeResponse>(`${this.baseUrl}/import`, formData)
      .pipe(catchError(this.handleError));
  }

  importByText(xmlText: string): Observable<ImportNfeResponse> {
    const formData = new FormData();
    formData.append('XmlText', xmlText);

    return this.http
      .post<ImportNfeResponse>(`${this.baseUrl}/import`, formData)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    const apiError = error.error as ApiErrorResponse

    return throwError(() => apiError);
  }
}
