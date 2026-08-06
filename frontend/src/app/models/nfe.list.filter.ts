import { TipoNota } from './enums/tipo.nota';

export interface NfeListFilter {
  search?: string;
  tipo?: TipoNota | null;
  emitente?: string;
  dataDe?: string;
  dataAte?: string;
  page?: number;
  pageSize?: number;
}
