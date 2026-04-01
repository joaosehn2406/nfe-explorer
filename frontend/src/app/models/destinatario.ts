export interface Destinatario {
  razaoSocial: string;
  cnpj: string | null;
  cpf: string | null;
  inscricaoEstadual: string | null;
  municipio: string;
  cep: string;
}
