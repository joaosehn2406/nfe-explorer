export interface TopEmitente {
  nome: string;
  valor: number;
}

export interface NotasPorMes {
  ano: number;
  mes: number;
  valor: number;
}

export interface DashboardStats {
  totalNotas: number;
  valorTotal: number;
  totalSaidas: number;
  totalEntradas: number;
  topEmitentes: TopEmitente[];
  notasPorMes: NotasPorMes[];
}
