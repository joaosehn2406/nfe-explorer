import { InvoiceType } from '../enums/invoice-type';

export interface NfeListItem {
  id: string;
  invoiceNumber: string;
  series: string;
  accessKey: string;
  invoiceType: InvoiceType;
  issuedAt: string;
  totalAmount: number;
  issuerName: string;
  issuerCnpj: string | null;
  recipientName: string;
}
