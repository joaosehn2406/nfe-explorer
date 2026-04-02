import {Component, signal} from '@angular/core';
import { ImportCardComponent } from './import-card/import.card.component';
import { NfeService } from '../../services/nfe.service';
import { ImportNfeResponse } from '../../models/response/import.nfe.response';
import { ApiErrorResponse } from '../../models/response/api.error.response';

@Component({
  selector: 'app-import',
  imports: [
    ImportCardComponent
  ],
  templateUrl: './import.component.html',
  styleUrl: './import.component.css'
})
export class ImportComponent {

  importResult = signal<ImportNfeResponse | null>(null);
  errorMessage = signal<string | null>(null);

  constructor(private nfeService: NfeService) {
  }

  onXmlSubmitted(payload: string | File) {
    this.errorMessage.set(null)

    const request$ = payload instanceof File
      ? this.nfeService.importByFile(payload)
      : this.nfeService.importByText(payload);

    request$.subscribe({
      next: (result) => {
        this.importResult.set(result)
      },
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message)
      }
    })
  }
}
