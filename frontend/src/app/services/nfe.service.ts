import { HttpClient, HttpEventType, HttpParams, HttpRequest } from '@angular/common/http';
import { filter, map, Observable } from 'rxjs';
import { ImportNfeResponse } from '../models/response/import-nfe.response';
import { Injectable } from '@angular/core';
import { NfeDetailsResponse } from '../models/response/nfe-details.response';
import { NfeListItem } from '../models/response/nfe-list-item';
import { PagedResponse } from '../models/response/paged.response';
import { NfeListFilter } from '../models/nfe-list-filter';
import { DashboardStats } from '../models/response/dashboard.stats';
import { ImportLog } from '../models/response/import-log';
import { ImportStatus } from '../models/enums/import-status';

@Injectable({ providedIn: 'root' })
export class NfeService {
  private readonly baseUrl = '/api/nfe_explorer';

  constructor(private http: HttpClient) {}

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

  getInvoices(filter: NfeListFilter): Observable<PagedResponse<NfeListItem>> {
    let params = new HttpParams();

    if (filter.search) params = params.set('search', filter.search);
    if (filter.type !== null && filter.type !== undefined) params = params.set('type', filter.type);
    if (filter.issuer) params = params.set('issuer', filter.issuer);
    if (filter.issuedFrom) params = params.set('issuedFrom', filter.issuedFrom);
    if (filter.issuedTo) params = params.set('issuedTo', filter.issuedTo);
    params = params.set('page', filter.page ?? 1);
    params = params.set('pageSize', filter.pageSize ?? 10);

    return this.http.get<PagedResponse<NfeListItem>>(this.baseUrl, { params });
  }

  getIssuers(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/issuers`);
  }

  getDashboard(): Observable<DashboardStats> {
    return this.http.get<DashboardStats>(`${this.baseUrl}/dashboard`);
  }

  getHistory(status?: ImportStatus | null): Observable<ImportLog[]> {
    let params = new HttpParams();
    if (status !== null && status !== undefined) params = params.set('status', status);
    return this.http.get<ImportLog[]>(`${this.baseUrl}/history`, { params });
  }

  deleteNfe(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
