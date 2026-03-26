import { Routes } from '@angular/router';
import { LayoutComponent } from './features/layout/layout.component';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'importNFe', pathMatch: 'full' },
      //{ path: 'importNFe', component: ImportNFeComponent }
    ]
  },
];
