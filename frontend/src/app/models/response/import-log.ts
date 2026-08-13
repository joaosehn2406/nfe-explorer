import { ImportStatus } from '../enums/import-status';

export interface ImportLog {
  id: string;
  timestamp: string;
  status: ImportStatus;
  fileName: string;
  invoiceNumber: string | null;
  issuer: string | null;
  amount: number | null;
  message: string | null;
}
