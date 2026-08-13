import { Component, OnInit, computed, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { NfeListItem } from '../../models/response/nfe-list-item';
import { NfeListFilter } from '../../models/nfe-list-filter';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { InvoiceType } from '../../models/enums/invoice-type';
import { formatCnpj, invoiceTypeLabel, isOutbound } from '../../shared/format';

@Component({
  selector: 'app-invoice-list',
  imports: [CurrencyPipe, DatePipe, FormsModule, RouterLink],
  templateUrl: './invoice-list.component.html',
  styleUrl: './invoice-list.component.css',
})
export class InvoiceListComponent implements OnInit {
  items = signal<NfeListItem[]>([]);
  total = signal(0);
  totalPages = signal(1);
  page = signal(1);
  pageSize = 10;
  loading = signal(false);
  error = signal<string | null>(null);
  issuers = signal<string[]>([]);

  search = '';
  typeFilter: '' | 'outbound' | 'inbound' = '';
  issuerFilter = '';
  issuedFrom = '';
  issuedTo = '';

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
    this.nfeService.getIssuers().subscribe({
      next: list => this.issuers.set(list),
      error: () => this.issuers.set([]),
    });
    this.load();
  }

  private buildFilter(): NfeListFilter {
    let type: InvoiceType | null = null;
    if (this.typeFilter === 'outbound') type = InvoiceType.Outbound;
    else if (this.typeFilter === 'inbound') type = InvoiceType.Inbound;

    return {
      search: this.search.trim() || undefined,
      type,
      issuer: this.issuerFilter || undefined,
      issuedFrom: this.issuedFrom || undefined,
      issuedTo: this.issuedTo ? `${this.issuedTo}T23:59:59` : undefined,
      page: this.page(),
      pageSize: this.pageSize,
    };
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);

    this.nfeService
      .getInvoices(this.buildFilter())
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: res => {
          this.items.set(res.items);
          this.total.set(res.total);
          this.totalPages.set(Math.max(1, res.totalPages));
          if (this.page() > res.totalPages && res.totalPages > 0) {
            this.page.set(res.totalPages);
            this.load();
          }
        },
        error: (err: ApiErrorResponse) => {
          this.error.set(err.message ?? 'Could not load invoices.');
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
    this.typeFilter = '';
    this.issuerFilter = '';
    this.issuedFrom = '';
    this.issuedTo = '';
    this.applyFilters();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages() || page === this.page()) return;
    this.page.set(page);
    this.load();
  }

  open(invoice: NfeListItem): void {
    void this.router.navigate(['/invoices', invoice.id]);
  }

  protected readonly formatCnpj = formatCnpj;
  protected readonly isOutbound = isOutbound;
  protected readonly invoiceTypeLabel = invoiceTypeLabel;
}
