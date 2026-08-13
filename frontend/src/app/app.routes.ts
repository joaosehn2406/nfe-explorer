import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { ImportComponent } from './features/import/import.component';
import { InvoiceDetailsComponent } from './features/invoice-details/invoice-details.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { InvoiceListComponent } from './features/invoice-list/invoice-list.component';
import { ImportHistoryComponent } from './features/import-history/import-history.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: 'Overview' } },
      { path: 'import', component: ImportComponent, data: { title: 'Import NFe' } },
      { path: 'invoices', component: InvoiceListComponent, data: { title: 'Invoices' } },
      { path: 'invoices/:id', component: InvoiceDetailsComponent, data: { title: 'Invoice details' } },
      { path: 'history', component: ImportHistoryComponent, data: { title: 'Import history' } },
      { path: '**', redirectTo: 'dashboard' },
    ],
  },
];
