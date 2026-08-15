import { Component, OnInit, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NfeService } from '../../services/nfe.service';
import { TranslatePipe } from '../../shared/translate.pipe';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, TranslatePipe],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent implements OnInit {
  totalInvoices = signal<number | null>(null);

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.nfeService.getInvoices({ page: 1, pageSize: 1 }).subscribe({
      next: (res) => this.totalInvoices.set(res.total),
      error: () => this.totalInvoices.set(null),
    });
  }
}
