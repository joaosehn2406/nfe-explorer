import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ImportNfeResponse } from '../../models/response/import-nfe.response';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { ImportCardComponent } from './import-card/import.card.component';
import { MatProgressBar } from '@angular/material/progress-bar';
import { TranslatePipe } from '../../shared/translate.pipe';
import { LanguageService } from '../../services/language.service';
import { translate } from '../../shared/translations';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  imports: [
    ImportCardComponent,
    MatProgressBar,
    TranslatePipe
  ],
  styleUrl: './import.component.css'
})
export class ImportComponent {
  importResult = signal<ImportNfeResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);

  constructor(
    private nfeService: NfeService,
    private router: Router,
    private languageService: LanguageService
  ) {}

  onXmlSubmitted(payload: string | File) {
    this.errorMessage.set(null);
    this.importResult.set(null);
    this.isLoading.set(true);

    this.nfeService.importNfeRequest(payload).pipe(
      finalize(() => this.isLoading.set(false))
    ).subscribe({
      next: result => {
        this.importResult.set(result);
      },
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message ?? this.text('errors.unexpectedTryAgain'));
      }
    });
  }

  goToDetails(): void {
    const id = this.importResult()?.id;

    if (!id) return;

    this.router.navigate(['/invoices', id]);
  }

  private text(key: string): string {
    return translate(key, this.languageService.getLanguage());
  }
}
