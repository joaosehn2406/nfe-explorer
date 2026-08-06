import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { ImportComponent } from './features/import/import.component';
import { ImportDetailsComponent } from './features/import-details/import.details.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { ListaComponent } from './features/lista/lista.component';
import { HistoricoComponent } from './features/historico/historico.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: 'Visão geral' } },
      { path: 'importar', component: ImportComponent, data: { title: 'Importar NF-e' } },
      { path: 'notas', component: ListaComponent, data: { title: 'Notas fiscais' } },
      { path: 'notas/:id', component: ImportDetailsComponent, data: { title: 'Detalhes da nota' } },
      { path: 'historico', component: HistoricoComponent, data: { title: 'Histórico de importações' } },
      { path: '**', redirectTo: 'dashboard' },
    ],
  },
];
