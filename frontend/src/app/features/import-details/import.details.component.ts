import { Component, OnInit, signal } from '@angular/core';
import { NfeDetailsResponse } from '../../models/response/nfe.details.response';
import { ActivatedRoute, Router } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { ApiErrorResponse } from '../../models/response/api.error.response';
import { finalize } from 'rxjs';
import { DatePipe } from '@angular/common';
import { tipoNotaResolver } from '../../models/enums/tipo.nota';
import { BadgeComponent } from '../utils/badge/badge.component';
import { MatTab, MatTabGroup } from '@angular/material/tabs';

@Component({
  selector: 'app-import-details',
  imports: [
    DatePipe,
    BadgeComponent,
    MatTab,
    MatTabGroup
  ],
  templateUrl: './import.details.component.html',
  styleUrl: './import.details.component.css'
})
export class ImportDetailsComponent implements OnInit {
  importDetails = signal<NfeDetailsResponse | null>(null);
  errorMessage = signal<string | null>(null);
  isLoading = signal<boolean>(false);
  copySuccess = signal<boolean>(false);
  copySuccessLeaving = signal(false);

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
    const accessKey = this.importDetails()?.nfe?.chaveAcesso;

    if (!accessKey) return;

    navigator.clipboard.writeText(accessKey);

    this.copySuccess.set(true);
    this.copySuccessLeaving.set(false);

    setTimeout(() => {
      this.copySuccessLeaving.set(true);
    }, 1800);

    setTimeout(() => {
      this.copySuccess.set(false);
      this.copySuccessLeaving.set(false);
    }, 2100);
  }

  goBack(): void {
    void this.router.navigate(['/importar']);
  }

  protected readonly tipoNotaResolver = tipoNotaResolver;
}
