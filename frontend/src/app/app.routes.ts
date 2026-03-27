import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';
import { ImportComponent } from './features/import/import.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'importar', pathMatch: 'full' },
      { path: 'importar', component: ImportComponent, data: { title: 'Importar NF-e' } }
    ]
  },
];
