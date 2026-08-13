import { Component, OnInit, computed, signal } from '@angular/core';
import { NfeDetailsResponse } from '../../models/response/nfe-details.response';
import { ActivatedRoute, Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { PaymentMethod } from '../../models/enums/payment-method';
import { DetailsCardComponent } from '../utils/details-card/details-card.component';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
import { formatCnpj, formatCpf, groupAccessKey, invoiceTypeLabel, isOutbound } from '../../shared/format';

@Component({
  selector: 'app-invoice-details',
  imports: [DatePipe, CurrencyPipe, DetailsCardComponent, MatTab, MatTabGroup],
  templateUrl: './invoice-details.component.html',
  styleUrl: './invoice-details.component.css',
})
export class InvoiceDetailsComponent implements OnInit {
  invoiceDetails = signal<NfeDetailsResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  copySuccess = signal<boolean>(false);
  copySuccessLeaving = signal(false);
  showDeleteModal = signal(false);
  deleting = signal(false);

  taxRows = computed(() => {
    const taxes = this.invoiceDetails()?.nfe?.taxes;
    if (!taxes) return [];
    const max = Math.max(taxes.icmsAmount, taxes.pisAmount, taxes.cofinsAmount, 1);
    return [
      { label: 'ICMS', amount: taxes.icmsAmount, color: 'var(--accent)', pct: (taxes.icmsAmount / max) * 100 },
      { label: 'PIS', amount: taxes.pisAmount, color: 'var(--inbound)', pct: (taxes.pisAmount / max) * 100 },
      { label: 'COFINS', amount: taxes.cofinsAmount, color: 'var(--outbound)', pct: (taxes.cofinsAmount / max) * 100 },
    ];
  });

  taxBurden = computed(() => {
    const taxes = this.invoiceDetails()?.nfe?.taxes;
    if (!taxes || taxes.invoiceAmount === 0) return '0.00';
    return ((taxes.totalTaxesAmount / taxes.invoiceAmount) * 100).toFixed(2);
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private nfeService: NfeService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.errorMessage.set('NFe ID was not found.');
      return;
    }
    this.loadDetails(id);
  }

  loadDetails(id: string): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.nfeService.getNfeDetails(id).pipe(
      finalize(() => this.isLoading.set(false))
    ).subscribe({
      next: response => this.invoiceDetails.set(response),
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message ?? 'Could not load NFe details.');
      }
    });
  }

  copyAccessKey(): void {
    const accessKey = this.invoiceDetails()?.nfe?.accessKey;
    if (!accessKey) return;

    navigator.clipboard.writeText(accessKey);
    this.copySuccess.set(true);
    this.copySuccessLeaving.set(false);

    setTimeout(() => this.copySuccessLeaving.set(true), 1800);
    setTimeout(() => {
      this.copySuccess.set(false);
      this.copySuccessLeaving.set(false);
    }, 2100);
  }

  confirmDelete(): void {
    const id = this.invoiceDetails()?.nfe?.id;
    if (!id) return;

    this.deleting.set(true);
    this.nfeService.deleteNfe(id).pipe(
      finalize(() => this.deleting.set(false))
    ).subscribe({
      next: () => {
        this.showDeleteModal.set(false);
        void this.router.navigate(['/invoices']);
      },
      error: (err: ApiErrorResponse) => {
        this.showDeleteModal.set(false);
        this.errorMessage.set(err.message ?? 'Could not delete the invoice.');
      }
    });
  }

  goBack(): void {
    void this.router.navigate(['/invoices']);
  }

  formatPaymentMethod(method: PaymentMethod | undefined): string {
    const map: Record<number, string> = {
      1: 'Cash', 2: 'Check', 3: 'Credit card', 4: 'Debit card',
      5: 'Store credit', 10: 'Meal voucher', 11: 'Food voucher',
      12: 'Gift voucher', 13: 'Fuel voucher', 15: 'Bank slip',
      90: 'No payment', 99: 'Other'
    };
    return method !== undefined ? (map[method] ?? 'Other') : '-';
  }

  protected readonly formatCnpj = formatCnpj;
  protected readonly formatCpf = formatCpf;
  protected readonly groupAccessKey = groupAccessKey;
  protected readonly isOutbound = isOutbound;
  protected readonly invoiceTypeLabel = invoiceTypeLabel;
}
