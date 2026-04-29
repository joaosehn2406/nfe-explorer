import { Component, OnInit, signal } from '@angular/core';
import { NfeDetailsResponse } from '../../models/response/nfe.details.response';
import { ActivatedRoute, Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { DatePipe, formatDate } from '@angular/common';

@Component({
  selector: 'app-import-details',
  imports: [
    DatePipe
  ],
  templateUrl: './import.details.component.html',
  styleUrl: './import.details.component.css'
})
export class ImportDetailsComponent implements OnInit {
  importDetails = signal<NfeDetailsResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  copySuccess = signal<boolean>(false);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private nfeService: NfeService
  ) {
  }

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
      next: (response) => {
        this.importDetails.set(response);
      },
      error: (err: ApiErrorResponse) => {
        this.errorMessage.set(err.message ?? 'Erro ao carregar detalhes da NF-e.');
      }
    });
  }

  copyAccessKey(): void {
    const chaveAcesso = this.importDetails()?.nfe?.chaveAcesso;

    if (!chaveAcesso) {
      this.errorMessage.set('Chave de acesso não encontrada.');
      return;
    }

    navigator.clipboard.writeText(chaveAcesso)
      .then(() => {
        this.copySuccess.set(true)

        setTimeout(() => {
          this.copySuccess.set(false)
        }, 2000)
      })
  }

  goBack(): void {
    void this.router.navigate(['/importar']);
  }

  protected readonly formatDate = formatDate;
}
