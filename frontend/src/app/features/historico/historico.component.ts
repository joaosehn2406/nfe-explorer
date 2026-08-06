import { Component, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { ImportLog } from '../../models/response/import.log';
import { StatusImportacao } from '../../models/enums/status.importacao';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { statusLabel } from '../../shared/format';

@Component({
  selector: 'app-historico',
  imports: [CurrencyPipe, DatePipe, FormsModule],
  templateUrl: './historico.component.html',
  styleUrl: './historico.component.css',
})
export class HistoricoComponent implements OnInit {
  logs = signal<ImportLog[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  statusFilter: '' | '0' | '1' | '2' = '';

  readonly Status = StatusImportacao;

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    const status = this.statusFilter === '' ? null : (Number(this.statusFilter) as StatusImportacao);

    this.nfeService
      .getHistorico(status)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (res) => this.logs.set(res),
        error: (err: ApiErrorResponse) => {
          this.error.set(err.message ?? 'Não foi possível carregar o histórico.');
          this.logs.set([]);
        },
      });
  }

  statusClass(status: StatusImportacao): string {
    switch (status) {
      case StatusImportacao.Sucesso: return 'ok';
      case StatusImportacao.Erro: return 'err';
      case StatusImportacao.Duplicada: return 'warn';
      default: return 'neutral';
    }
  }

  protected readonly statusLabel = statusLabel;
}
