import { Component, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { ImportLog } from '../../models/response/import-log';
import { ImportStatus } from '../../models/enums/import-status';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { statusLabel } from '../../shared/format';

@Component({
  selector: 'app-import-history',
  imports: [CurrencyPipe, DatePipe, FormsModule],
  templateUrl: './import-history.component.html',
  styleUrl: './import-history.component.css',
})
export class ImportHistoryComponent implements OnInit {
  logs = signal<ImportLog[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  statusFilter: '' | '0' | '1' | '2' = '';

  readonly Status = ImportStatus;

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    const status = this.statusFilter === '' ? null : (Number(this.statusFilter) as ImportStatus);

    this.nfeService
      .getHistory(status)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: res => this.logs.set(res),
        error: (err: ApiErrorResponse) => {
          this.error.set(err.message ?? 'Could not load import history.');
          this.logs.set([]);
        },
      });
  }

  statusClass(status: ImportStatus): string {
    switch (status) {
      case ImportStatus.Success: return 'ok';
      case ImportStatus.Error: return 'err';
      case ImportStatus.Duplicate: return 'warn';
      default: return 'neutral';
    }
  }

  protected readonly statusLabel = statusLabel;
}
