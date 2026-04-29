import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ImportNfeResponse } from '../../models/response/import.nfe.response';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { ImportCardComponent } from './import-card/import.card.component';
import { MatProgressBar } from '@angular/material/progress-bar';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  imports: [
    ImportCardComponent,
    MatProgressBar
  ],
  styleUrl: './import.component.css'
})
export class ImportComponent {

  importResult = signal<ImportNfeResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);

  constructor(
    private nfeService: NfeService,
    private router: Router
  ) {
  }

  onXmlSubmitted(payload: string | File) {
    this.errorMessage.set(null);
    this.importResult.set(null);
    this.isLoading.set(true);

    this.nfeService.importNfeRequest(payload).pipe(
      finalize(() => this.isLoading.set(false))
    ).subscribe({
      next: (result) => {
        this.importResult.set(result);
      },
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message);
      }
    });
  }

  goToDetails(): void {
    const id = this.importResult()?.id;

    if (!id) {
      return;
    }

    this.router.navigate(['/import-details', id]);
  }
}
