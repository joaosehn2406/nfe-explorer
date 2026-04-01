import {Emitente} from '../emitente';
import {Destinatario} from '../destinatario';
import {Produto} from '../produto';
import {ImpostosNfe} from '../impostos.nfe';
import {Transportadora} from '../transportadora';
import {TipoNota} from '../enums/tipo.nota';
import {FormaPagamento} from '../enums/forma.pagamento';

export interface NfeResponse {
  id: string;
  chaveAcesso: string;
  naturezaOperacao: string;
  numeroNota: string;
  serie: string;
  valorTotal: number;
  valorPago: number;
  formaPagamento: FormaPagamento;
  tipoNota: TipoNota;
  dataEmissao: string;
  emitente: Emitente;
  destinatario: Destinatario;
  produtos: Produto[];
  impostos: ImpostosNfe;
  transportadora: Transportadora;
}
