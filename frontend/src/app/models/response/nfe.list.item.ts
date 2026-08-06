import { TipoNota } from '../enums/tipo.nota';

export interface NfeListItem {
  id: string;
  numeroNota: string;
  serie: string;
  chaveAcesso: string;
  tipoNota: TipoNota;
  dataEmissao: string;
  valorTotal: number;
  emitenteNome: string;
  emitenteCnpj: string | null;
  destinatarioNome: string;
}
