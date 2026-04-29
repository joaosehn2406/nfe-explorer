import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { ImportComponent } from './features/import/import.component';
import { ImportDetailsComponent } from './features/import-details/import.details.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'importar', pathMatch: 'full' },
      { path: 'importar', component: ImportComponent, data: { title: 'Importar NF-e' } },
      { path: 'import-details/:id', component: ImportDetailsComponent, data: { title: 'Detalhes da NF-e' } }
    ]
  }
];
