import {ModalidadeFrete} from './enums/modalidade.frete';

export interface Transportadora {
  id: string;
  razaoSocial: string;
  cnpj: string | null;
  cpf: string | null;
  inscricaoEstadual: string | null;
  municipio: string | null;
  uf: string | null;
  modalidadeFrete: ModalidadeFrete;
}
