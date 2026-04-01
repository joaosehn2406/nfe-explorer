import { Component } from '@angular/core';
import {SAMPLE_XML} from '../import-example';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-import-card',
  imports: [
    FormsModule
  ],
  templateUrl: './import.card.component.html',
  styleUrl: './import.card.component.css',
})
export class ImportCardComponent {

  xmlTextAreaContent = '';

  fillTextAreaWithExample() {
    this.xmlTextAreaContent = SAMPLE_XML;
  }

  removeTextFromTextArea() {
    this.xmlTextAreaContent = ''
  }
}
