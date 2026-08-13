import { Component, OnInit, computed, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { DashboardStats } from '../../models/response/dashboard.stats';
import { ApiErrorResponse } from '../../models/response/api.error.response';

const MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
const BAR_COLORS = ['var(--accent)', 'var(--inbound)', 'var(--outbound)', 'var(--ok)', 'var(--warn)'];

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

  average = computed(() => {
    const stats = this.stats();
    if (!stats || stats.totalInvoices === 0) return 0;
    return stats.totalAmount / stats.totalInvoices;
  });

  pctOutbound = computed(() => this.pct(this.stats()?.totalOutbound));
  pctInbound = computed(() => this.pct(this.stats()?.totalInbound));

  donut = computed(() => {
    const stats = this.stats();
    const totalTypes = (stats?.totalOutbound ?? 0) + (stats?.totalInbound ?? 0);
    const outbound = totalTypes > 0 ? ((stats?.totalOutbound ?? 0) / totalTypes) * 100 : 0;
    return { outbound, inbound: 100 - outbound };
  });

  topIssuers = computed(() => {
    const list = this.stats()?.topIssuers ?? [];
    const max = list.length ? Math.max(...list.map(issuer => issuer.amount)) : 1;
    return list.map((issuer, i) => ({
      ...issuer,
      pct: (issuer.amount / max) * 100,
      color: BAR_COLORS[i % BAR_COLORS.length],
    }));
  });

  months = computed(() => {
    const raw = this.stats()?.monthlyInvoices ?? [];
    const buckets = raw.slice(-12);
    const max = buckets.length ? Math.max(...buckets.map(month => month.amount)) : 1;
    return buckets.map(month => ({
      label: MONTHS[month.month - 1] ?? '-',
      year: String(month.year).slice(2),
      amount: month.amount,
      heightPct: max > 0 ? Math.max((month.amount / max) * 100, 2) : 2,
    }));
  });

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.loading.set(true);
    this.nfeService
      .getDashboard()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: res => this.stats.set(res),
        error: (err: ApiErrorResponse) => this.error.set(err.message ?? 'Could not load dashboard.'),
      });
  }

  private pct(value: number | undefined): string {
    const stats = this.stats();
    if (!stats || stats.totalInvoices === 0 || value === undefined) return '0';
    return ((value / stats.totalInvoices) * 100).toFixed(1);
  }
}
