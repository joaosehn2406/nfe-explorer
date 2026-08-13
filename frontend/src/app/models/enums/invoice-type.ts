export enum InvoiceType {
  Inbound = 0,
  Outbound = 1,
}

export function invoiceTypeResolver(id: unknown): string {
  return id === InvoiceType.Outbound || id === 1 ? 'Outbound' : 'Inbound';
}
