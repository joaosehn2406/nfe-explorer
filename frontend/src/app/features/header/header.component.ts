import { Component, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-header',
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  title = signal<string>('');

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => {
          let child = this.route.firstChild;
          while (child?.firstChild) child = child.firstChild;
          return child?.snapshot.data['title'] as string | undefined;
        })
      )
      .subscribe((title) => this.title.set(title ?? ''));
  }
}
