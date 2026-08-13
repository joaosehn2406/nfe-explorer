import { InvoiceType } from '../models/enums/invoice-type';
import { ImportStatus } from '../models/enums/import-status';

export function formatCnpj(cnpj: string | null | undefined): string {
  if (!cnpj) return '-';
  return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
}

export function formatCpf(cpf: string | null | undefined): string {
  if (!cpf) return '-';
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
}

export function groupAccessKey(accessKey: string | null | undefined): string {
  if (!accessKey) return '-';
  return accessKey.replace(/\s+/g, '').replace(/(.{4})/g, '$1 ').trim();
}

export function isOutbound(type: InvoiceType | number | undefined | null): boolean {
  return type === InvoiceType.Outbound;
}

export function invoiceTypeLabel(type: InvoiceType | number | undefined | null): string {
  return type === InvoiceType.Outbound ? 'Outbound' : 'Inbound';
}

export function statusLabel(status: ImportStatus): string {
  switch (status) {
    case ImportStatus.Success: return 'Imported';
    case ImportStatus.Error: return 'Failed';
    case ImportStatus.Duplicate: return 'Duplicate';
    default: return '-';
  }
}
