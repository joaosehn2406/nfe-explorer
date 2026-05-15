import { Component, Input } from '@angular/core';
import { NgStyle } from '@angular/common';

@Component({
  selector: 'app-badge',
  imports: [
    NgStyle
  ],
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.css',
})
export class BadgeComponent {
  @Input() text: string | null = ''

  @Input() backgroundColor = '#ffffff'
  @Input() textColor = '#ffffff'
  @Input() borderColor = '#ffffff'
}
