import { Issuer } from '../issuer';
import { Recipient } from '../recipient';
import { Product } from '../product';
import { NfeTaxes } from '../nfe-taxes';
import { Carrier } from '../carrier';
import { InvoiceType } from '../enums/invoice-type';
import { PaymentMethod } from '../enums/payment-method';

export interface NfeResponse {
  id: string;
  accessKey: string;
  operationNature: string;
  invoiceNumber: string;
  series: string;
  totalAmount: number;
  paidAmount: number;
  paymentMethod: PaymentMethod;
  invoiceType: InvoiceType;
  issuedAt: string;
  issuer: Issuer;
  recipient: Recipient;
  products: Product[];
  taxes: NfeTaxes;
  carrier: Carrier | null;
}
