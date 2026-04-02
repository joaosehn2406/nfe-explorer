import { Component } from '@angular/core';
import { ImportCardComponent } from './import-card/import.card.component';
import { NfeService } from '../../services/nfe.service';
import { ImportNfeResponse } from '../../models/response/import.nfe.response';

@Component({
  selector: 'app-import',
  imports: [
    ImportCardComponent
  ],
  templateUrl: './import.component.html',
  styleUrl: './import.component.css'
})
export class ImportComponent {

  importResult: ImportNfeResponse | null = null
  errorMessage: Error | null = null

  constructor(private nfeService: NfeService) {
  }

  onXmlSubmitted(payload: string | File) {
    const request$ = payload instanceof File
      ? this.nfeService.importByFile(payload)
      : this.nfeService.importByText(payload);

    request$.subscribe({
      next: (result) => {
        this.importResult = result
      },
      error: (err: Error) => {
        this.errorMessage = err
      }
    })
  }
}
