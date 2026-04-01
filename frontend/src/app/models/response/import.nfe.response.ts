import {TipoNota} from '../enums/tipo.nota';

export interface ImportNfeResponse {
  id: string;
  numeroNota: string;
  emitente: string;
  valorTotal: number;
  tipoNota: TipoNota;
}
