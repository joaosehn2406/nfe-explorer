import { Component } from '@angular/core';
import { SidebarComponent } from './features/sidebar/sidebar.component';
import {HeaderComponent} from './features/header/header.component';

@Component({
  selector: 'app-root',
  imports: [SidebarComponent, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
}
