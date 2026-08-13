import { InvoiceType } from './enums/invoice-type';

export interface NfeListFilter {
  search?: string;
  type?: InvoiceType | null;
  issuer?: string;
  issuedFrom?: string;
  issuedTo?: string;
  page?: number;
  pageSize?: number;
}
