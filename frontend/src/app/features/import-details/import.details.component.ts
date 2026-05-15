import { Component, OnInit, computed, signal } from '@angular/core';
import { NfeDetailsResponse } from '../../models/response/nfe.details.response';
import { ActivatedRoute, Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { tipoNotaResolver } from '../../models/enums/tipo.nota';
import { FormaPagamento } from '../../models/enums/forma.pagamento';
import { BadgeComponent } from '../utils/badge/badge.component';
import { DetailsCardComponent } from '../utils/details-card/details-card.component';
import { MatTab, MatTabGroup } from '@angular/material/tabs';

@Component({
  selector: 'app-import-details',
  imports: [
    DatePipe,
    CurrencyPipe,
    BadgeComponent,
    DetailsCardComponent,
    MatTab,
    MatTabGroup
  ],
  templateUrl: './import.details.component.html',
  styleUrl: './import.details.component.css'
})
export class ImportDetailsComponent implements OnInit {
  importDetails = signal<NfeDetailsResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  copySuccess = signal<boolean>(false);
  copySuccessLeaving = signal(false);

  impostosRows = computed(() => {
    const imp = this.importDetails()?.nfe?.impostos;
    if (!imp) return [];
    const max = Math.max(imp.valorICMS, imp.valorPIS, imp.valorCOFINS, 1);
    return [
      { label: 'ICMS', valor: imp.valorICMS, color: '#2d6a4f', pct: (imp.valorICMS / max) * 100 },
      { label: 'PIS', valor: imp.valorPIS, color: '#2563a8', pct: (imp.valorPIS / max) * 100 },
      { label: 'COFINS', valor: imp.valorCOFINS, color: '#c08b2a', pct: (imp.valorCOFINS / max) * 100 },
    ];
  });

  cargaTributaria = computed(() => {
    const imp = this.importDetails()?.nfe?.impostos;
    if (!imp || imp.valorNota === 0) return '0.00';
    return ((imp.valorTotalTributos / imp.valorNota) * 100).toFixed(2);
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private nfeService: NfeService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.errorMessage.set('ID da NF-e não encontrado.');
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
      next: (response) => this.importDetails.set(response),
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message ?? 'Erro ao carregar detalhes da NF-e.');
      }
    });
  }

  copyAccessKey(): void {
    const accessKey = this.importDetails()?.nfe?.chaveAcesso;
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

  goBack(): void {
    void this.router.navigate(['/importar']);
  }

  formatCnpj(cnpj: string | null | undefined): string {
    if (!cnpj) return '—';
    return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
  }

  formatCpf(cpf: string | null | undefined): string {
    if (!cpf) return '—';
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  formatPagamento(forma: FormaPagamento | undefined): string {
    const map: Record<number, string> = {
      1: 'Dinheiro', 2: 'Cheque', 3: 'Cartão Crédito', 4: 'Cartão Débito',
      5: 'Crédito Loja', 10: 'Vale Alimentação', 11: 'Vale Refeição',
      12: 'Vale Presente', 13: 'Vale Combustível', 15: 'Boleto',
      90: 'Sem Pagamento', 99: 'Outros'
    };
    return forma !== undefined ? (map[forma] ?? 'Outros') : '—';
  }

  protected readonly tipoNotaResolver = tipoNotaResolver;
}
