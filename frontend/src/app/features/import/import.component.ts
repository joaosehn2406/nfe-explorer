import {Component, signal} from '@angular/core';
import {ImportCardComponent} from './import-card/import.card.component';
import {NfeService} from '../../services/nfe.service';
import {ImportNfeResponse} from '../../models/response/import.nfe.response';
import {ApiErrorResponse} from '../../models/response/api.error.response';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {finalize} from 'rxjs';

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

  constructor(private nfeService: NfeService) {
  }

  onXmlSubmitted(payload: string | File) {
    this.errorMessage.set(null);
    this.importResult.set(null);
    this.isLoading.set(true);

    this.nfeService.importNfe(payload).pipe(
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
}
