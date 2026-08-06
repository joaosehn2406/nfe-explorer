import { Component, OnInit, computed, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { DashboardStats } from '../../models/response/dashboard.stats';
import { ApiErrorResponse } from '../../models/response/api.error.response';

const MESES = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
const BAR_COLORS = ['var(--accent)', 'var(--entrada)', 'var(--saida)', 'var(--ok)', 'var(--warn)'];

@Component({
  selector: 'app-dashboard',
  imports: [CurrencyPipe, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  stats = signal<DashboardStats | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  media = computed(() => {
    const s = this.stats();
    if (!s || s.totalNotas === 0) return 0;
    return s.valorTotal / s.totalNotas;
  });

  pctSaidas = computed(() => this.pct(this.stats()?.totalSaidas));
  pctEntradas = computed(() => this.pct(this.stats()?.totalEntradas));

  donut = computed(() => {
    const s = this.stats();
    const totalTipos = (s?.totalSaidas ?? 0) + (s?.totalEntradas ?? 0);
    const saida = totalTipos > 0 ? ((s?.totalSaidas ?? 0) / totalTipos) * 100 : 0;
    return { saida, entrada: 100 - saida };
  });

  topEmitentes = computed(() => {
    const list = this.stats()?.topEmitentes ?? [];
    const max = list.length ? Math.max(...list.map((e) => e.valor)) : 1;
    return list.map((e, i) => ({ ...e, pct: (e.valor / max) * 100, color: BAR_COLORS[i % BAR_COLORS.length] }));
  });

  meses = computed(() => {
    const raw = this.stats()?.notasPorMes ?? [];
    const buckets = raw.slice(-12);
    const max = buckets.length ? Math.max(...buckets.map((m) => m.valor)) : 1;
    return buckets.map((m) => ({
      label: MESES[m.mes - 1] ?? '—',
      ano: String(m.ano).slice(2),
      valor: m.valor,
      heightPct: max > 0 ? Math.max((m.valor / max) * 100, 2) : 2,
    }));
  });

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.loading.set(true);
    this.nfeService
      .getDashboard()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => this.stats.set(res),
        error: (err: ApiErrorResponse) => this.error.set(err.message ?? 'Não foi possível carregar o painel.'),
      });
  }

  private pct(value: number | undefined): string {
    const s = this.stats();
    if (!s || s.totalNotas === 0 || value === undefined) return '0';
    return ((value / s.totalNotas) * 100).toFixed(1);
  }
}
