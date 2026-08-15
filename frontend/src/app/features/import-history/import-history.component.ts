import { Component, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { NfeService } from '../../services/nfe.service';
import { ImportLog } from '../../models/response/import-log';
import { ImportStatus } from '../../models/enums/import-status';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { TranslatePipe } from '../../shared/translate.pipe';
import { LanguageService } from '../../services/language.service';
import { translate } from '../../shared/translations';

@Component({
  selector: 'app-import-history',
  imports: [CurrencyPipe, DatePipe, FormsModule, TranslatePipe],
  templateUrl: './import-history.component.html',
  styleUrl: './import-history.component.css',
})
export class ImportHistoryComponent implements OnInit {
  logs = signal<ImportLog[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  statusFilter: '' | '0' | '1' | '2' = '';

  readonly Status = ImportStatus;

  constructor(private nfeService: NfeService, private languageService: LanguageService) {}

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
          this.error.set(err.message ?? this.text('errors.historyLoad'));
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

  statusKey(status: ImportStatus): string {
    switch (status) {
      case ImportStatus.Success: return 'status.imported';
      case ImportStatus.Error: return 'status.failed';
      case ImportStatus.Duplicate: return 'status.duplicate';
      default: return '';
    }
  }

  private text(key: string): string {
    return translate(key, this.languageService.getLanguage());
  }
}
