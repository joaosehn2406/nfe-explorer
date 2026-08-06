import { TipoNota } from '../models/enums/tipo.nota';
import { StatusImportacao } from '../models/enums/status.importacao';

export function formatCnpj(cnpj: string | null | undefined): string {
  if (!cnpj) return '—';
  return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
}

export function formatCpf(cpf: string | null | undefined): string {
  if (!cpf) return '—';
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
}

/** Agrupa a chave de acesso (44 dígitos) em blocos de 4, como no DANFE. */
export function groupChave(chave: string | null | undefined): string {
  if (!chave) return '—';
  return chave.replace(/\s+/g, '').replace(/(.{4})/g, '$1 ').trim();
}

export function isSaida(tipo: TipoNota | number | undefined | null): boolean {
  return tipo === TipoNota.Saida;
}

export function tipoNotaLabel(tipo: TipoNota | number | undefined | null): string {
  return tipo === TipoNota.Saida ? 'Saída' : 'Entrada';
}

export function statusLabel(status: StatusImportacao): string {
  switch (status) {
    case StatusImportacao.Sucesso: return 'Importada';
    case StatusImportacao.Erro: return 'Falha';
    case StatusImportacao.Duplicada: return 'Duplicada';
    default: return '—';
  }
}
