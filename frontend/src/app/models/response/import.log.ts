import { StatusImportacao } from '../enums/status.importacao';

export interface ImportLog {
  id: string;
  dataHora: string;
  status: StatusImportacao;
  nomeArquivo: string;
  numeroNota: string | null;
  emitente: string | null;
  valor: number | null;
  mensagem: string | null;
}
