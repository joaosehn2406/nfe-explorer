import { InvoiceType } from '../enums/invoice-type';

export interface ImportNfeResponse {
  id: string;
  invoiceNumber: string;
  issuer: string;
  totalAmount: number;
  invoiceType: InvoiceType;
}
