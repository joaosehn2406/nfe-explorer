import { Component, OnInit, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NfeService } from '../../services/nfe.service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent implements OnInit {
  totalNotas = signal<number | null>(null);

  constructor(private nfeService: NfeService) {}

  ngOnInit(): void {
    this.nfeService.getNotas({ page: 1, pageSize: 1 }).subscribe({
      next: (res) => this.totalNotas.set(res.total),
      error: () => this.totalNotas.set(null),
    });
  }
}
