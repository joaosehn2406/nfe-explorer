import { Component, signal } from '@angular/core';
import { ImportCardComponent } from './import-card/import.card.component';
import { NfeService, UploadProgress } from '../../services/nfe.service';
import { ImportNfeResponse } from '../../models/response/import.nfe.response';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-import',
  imports: [ImportCardComponent, MatProgressBarModule],
  templateUrl: './import.component.html',
  styleUrl: './import.component.css'
})
export class ImportComponent {

  importResult = signal<ImportNfeResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  uploadProgress = signal<number>(0);

  constructor(private nfeService: NfeService) {
  }

  onXmlSubmitted(payload: string | File) {
    this.errorMessage.set(null);
    this.importResult.set(null);
    this.uploadProgress.set(0);
    this.isLoading.set(true);

    this.nfeService.importNfe(payload).subscribe({
      next: ({ progress, result }: UploadProgress) => {
        this.uploadProgress.set(progress);
        if (result) {
          this.importResult.set(result);
          this.isLoading.set(false);
        }
      },
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message);
        this.isLoading.set(false);
      }
    });
  }
}
