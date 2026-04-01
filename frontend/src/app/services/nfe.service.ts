import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import {ImportNfeResponse} from '../models/response/import.nfe.response';
import {Injectable} from '@angular/core';

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
    let message = 'Erro desconhecido ao processar a requisição.';

    if (error.status === 0) {
      message = 'Não foi possível conectar ao servidor. Verifique se o backend está rodando.';
    } else if (error.status === 400) {
      message = typeof error.error === 'string'
        ? error.error
        : error.error?.title || error.error?.message || 'Dados inválidos.';
    } else if (error.status === 404) {
      message = 'Nota fiscal não encontrada.';
    } else if (error.status >= 500) {
      message = 'Erro interno no servidor.';
    }

    return throwError(() => new Error(message));
  }
}
