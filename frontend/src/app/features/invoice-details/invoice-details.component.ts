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
import { formatCnpj, formatCpf, groupAccessKey, isOutbound } from '../../shared/format';
import { TranslatePipe } from '../../shared/translate.pipe';
import { LanguageService } from '../../services/language.service';
import { translate } from '../../shared/translations';
import { InvoiceType } from '../../models/enums/invoice-type';

@Component({
  selector: 'app-invoice-details',
  imports: [DatePipe, CurrencyPipe, DetailsCardComponent, MatTab, MatTabGroup, TranslatePipe],
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
    private nfeService: NfeService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.errorMessage.set(this.text('errors.detailsIdMissing'));
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
        this.errorMessage.set(err.message ?? this.text('errors.detailsLoad'));
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
        this.errorMessage.set(err.message ?? this.text('errors.deleteInvoice'));
      }
    });
  }

  goBack(): void {
    void this.router.navigate(['/invoices']);
  }

  invoiceTypeKey(type: InvoiceType | number | undefined | null): string {
    return isOutbound(type) ? 'invoiceType.outbound' : 'invoiceType.inbound';
  }

  paymentMethodKey(method: PaymentMethod | undefined): string {
    const map: Record<number, string> = {
      1: 'payment.cash',
      2: 'payment.check',
      3: 'payment.creditCard',
      4: 'payment.debitCard',
      5: 'payment.storeCredit',
      10: 'payment.mealVoucher',
      11: 'payment.foodVoucher',
      12: 'payment.giftVoucher',
      13: 'payment.fuelVoucher',
      15: 'payment.bankSlip',
      90: 'payment.noPayment',
      99: 'payment.other'
    };
    return method !== undefined ? (map[method] ?? 'payment.other') : '';
  }

  private text(key: string): string {
    return translate(key, this.languageService.getLanguage());
  }

  protected readonly formatCnpj = formatCnpj;
  protected readonly formatCpf = formatCpf;
  protected readonly groupAccessKey = groupAccessKey;
  protected readonly isOutbound = isOutbound;
}
