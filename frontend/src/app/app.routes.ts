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
      { path: 'dashboard', component: DashboardComponent, data: { titleKey: 'routes.overview' } },
      { path: 'import', component: ImportComponent, data: { titleKey: 'routes.importNfe' } },
      { path: 'invoices', component: InvoiceListComponent, data: { titleKey: 'routes.invoices' } },
      { path: 'invoices/:id', component: InvoiceDetailsComponent, data: { titleKey: 'routes.invoiceDetails' } },
      { path: 'history', component: ImportHistoryComponent, data: { titleKey: 'routes.importHistory' } },
      { path: '**', redirectTo: 'dashboard' },
    ],
  },
];
