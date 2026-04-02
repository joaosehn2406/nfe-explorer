import { Component, EventEmitter, Output } from '@angular/core';
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
  @Output() onClickAnalyse = new EventEmitter<File | string>();

  selectedFile: File | null = null
  xmlTextAreaContent = '';

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (file) this.selectedFile = file;
  }

  emitValue() {
    if (this.selectedFile) {
      this.onClickAnalyse.emit(this.selectedFile);
    } else if (this.xmlTextAreaContent.trim()) {
      this.onClickAnalyse.emit(this.xmlTextAreaContent.trim());
    }
  }

  fillTextAreaWithExample() {
    this.xmlTextAreaContent = SAMPLE_XML;
  }

  removeTextFromTextArea() {
    this.xmlTextAreaContent = ''
  }
}
