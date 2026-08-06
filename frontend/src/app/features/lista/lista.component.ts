import { Component, OnInit, computed, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { NfeListItem } from '../../models/response/nfe.list.item';
import { NfeListFilter } from '../../models/nfe.list.filter';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { TipoNota } from '../../models/enums/tipo.nota';
import { formatCnpj, isSaida, tipoNotaLabel } from '../../shared/format';

@Component({
  selector: 'app-lista',
  imports: [CurrencyPipe, DatePipe, FormsModule, RouterLink],
  templateUrl: './lista.component.html',
  styleUrl: './lista.component.css',
})
export class ListaComponent implements OnInit {
  items = signal<NfeListItem[]>([]);
  total = signal(0);
  totalPages = signal(1);
  page = signal(1);
  pageSize = 10;
  loading = signal(false);
  error = signal<string | null>(null);
  emitentes = signal<string[]>([]);

  search = '';
  tipoFilter: '' | 'saida' | 'entrada' = '';
  emitenteFilter = '';
  dataDe = '';
  dataAte = '';

  private searchDebounce?: ReturnType<typeof setTimeout>;

  pageWindow = computed(() => {
    const total = this.totalPages();
    const current = this.page();
    const span = 5;
    let start = Math.max(1, current - Math.floor(span / 2));
    let end = Math.min(total, start + span - 1);
    start = Math.max(1, end - span + 1);
    const pages: number[] = [];
    for (let i = start; i <= end; i++) pages.push(i);
    return pages;
  });

  rangeStart = computed(() => (this.total() === 0 ? 0 : (this.page() - 1) * this.pageSize + 1));
  rangeEnd = computed(() => Math.min(this.page() * this.pageSize, this.total()));

  constructor(private nfeService: NfeService, private router: Router) {}

  ngOnInit(): void {
    this.nfeService.getEmitentes().subscribe({
      next: (list) => this.emitentes.set(list),
      error: () => this.emitentes.set([]),
    });
    this.load();
  }

  private buildFilter(): NfeListFilter {
    let tipo: TipoNota | null = null;
    if (this.tipoFilter === 'saida') tipo = TipoNota.Saida;
    else if (this.tipoFilter === 'entrada') tipo = TipoNota.Entrada;

    return {
      search: this.search.trim() || undefined,
      tipo,
      emitente: this.emitenteFilter || undefined,
      dataDe: this.dataDe || undefined,
      dataAte: this.dataAte ? `${this.dataAte}T23:59:59` : undefined,
      page: this.page(),
      pageSize: this.pageSize,
    };
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.nfeService
      .getNotas(this.buildFilter())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => {
          this.items.set(res.items);
          this.total.set(res.total);
          this.totalPages.set(Math.max(1, res.totalPages));
          if (this.page() > res.totalPages && res.totalPages > 0) {
            this.page.set(res.totalPages);
            this.load();
          }
        },
        error: (err: ApiErrorResponse) => {
          this.error.set(err.message ?? 'Não foi possível carregar as notas.');
          this.items.set([]);
        },
      });
  }

  onSearchInput(): void {
    clearTimeout(this.searchDebounce);
    this.searchDebounce = setTimeout(() => this.applyFilters(), 300);
  }

  applyFilters(): void {
    this.page.set(1);
    this.load();
  }

  clearFilters(): void {
    this.search = '';
    this.tipoFilter = '';
    this.emitenteFilter = '';
    this.dataDe = '';
    this.dataAte = '';
    this.applyFilters();
  }

  goToPage(p: number): void {
    if (p < 1 || p > this.totalPages() || p === this.page()) return;
    this.page.set(p);
    this.load();
  }

  open(nota: NfeListItem): void {
    void this.router.navigate(['/notas', nota.id]);
  }

  protected readonly formatCnpj = formatCnpj;
  protected readonly isSaida = isSaida;
  protected readonly tipoNotaLabel = tipoNotaLabel;
}
