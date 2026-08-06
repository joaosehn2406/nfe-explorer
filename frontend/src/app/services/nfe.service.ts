import { HttpClient, HttpEventType, HttpParams, HttpRequest } from '@angular/common/http';
import { filter, map, Observable } from 'rxjs';
import { ImportNfeResponse } from '../models/response/import.nfe.response';
import { Injectable } from '@angular/core';
import { NfeDetailsResponse } from '../models/response/nfe.details.response';
import { NfeListItem } from '../models/response/nfe.list.item';
import { PagedResponse } from '../models/response/paged.response';
import { NfeListFilter } from '../models/nfe.list.filter';
import { DashboardStats } from '../models/response/dashboard.stats';
import { ImportLog } from '../models/response/import.log';
import { StatusImportacao } from '../models/enums/status.importacao';

@Injectable({ providedIn: 'root' })
export class NfeService {

  private readonly baseUrl = '/api/nfe_explorer';

  constructor(private http: HttpClient) {
  }

  importNfeRequest(payload: File | string): Observable<ImportNfeResponse> {
    const formData = new FormData();

    if (payload instanceof File) {
      formData.append('File', payload, payload.name);
    } else {
      formData.append('XmlText', payload);
    }

    const req = new HttpRequest('POST', `${this.baseUrl}/import`, formData);

    return this.http.request<ImportNfeResponse>(req).pipe(
      filter(event => event.type === HttpEventType.Response),
      map(event => (event as any).body as ImportNfeResponse)
    );
  }

  getNfeDetails(id: string): Observable<NfeDetailsResponse> {
    return this.http.get<NfeDetailsResponse>(`${this.baseUrl}/${id}`);
  }

  getNotas(filter: NfeListFilter): Observable<PagedResponse<NfeListItem>> {
    let params = new HttpParams();

    if (filter.search) params = params.set('search', filter.search);
    if (filter.tipo !== null && filter.tipo !== undefined) params = params.set('tipo', filter.tipo);
    if (filter.emitente) params = params.set('emitente', filter.emitente);
    if (filter.dataDe) params = params.set('dataDe', filter.dataDe);
    if (filter.dataAte) params = params.set('dataAte', filter.dataAte);
    params = params.set('page', filter.page ?? 1);
    params = params.set('pageSize', filter.pageSize ?? 10);

    return this.http.get<PagedResponse<NfeListItem>>(this.baseUrl, { params });
  }

  getEmitentes(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/emitentes`);
  }

  getDashboard(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/dashboard`);
  }

  getHistorico(status?: StatusImportacao | null): Observable<ImportLog[]> {
    let params = new HttpParams();
    if (status !== null && status !== undefined) params = params.set('status', status);
    return this.http.get<ImportLog[]>(`${this.baseUrl}/historico`, { params });
  }

  deleteNfe(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
