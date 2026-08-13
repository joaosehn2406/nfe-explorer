export interface TopIssuer {
  name: string;
  amount: number;
}

export interface MonthlyInvoices {
  year: number;
  month: number;
  amount: number;
}

export interface DashboardStats {
  totalInvoices: number;
  totalAmount: number;
  totalOutbound: number;
  totalInbound: number;
  topIssuers: TopIssuer[];
  monthlyInvoices: MonthlyInvoices[];
}
