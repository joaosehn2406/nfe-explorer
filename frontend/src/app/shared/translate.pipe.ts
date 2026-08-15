import {Pipe, PipeTransform, inject} from '@angular/core';
import {LanguageService} from '../services/language.service';
import {translate} from './translations';

@Pipe({
  name: 'translate',
  standalone: true
})
export class TranslatePipe implements PipeTransform {
  private readonly languageService = inject(LanguageService);

  transform(key: string | null | undefined): string {
    if (!key) return '';
    return translate(key, this.languageService.getLanguage());
  }
}
