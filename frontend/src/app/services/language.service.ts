import {Injectable} from '@angular/core';

@Injectable({ providedIn: 'root' })
export class LanguageService {
  getLanguage(): string {
    const browserLanguage = navigator.languages?.[0] ?? navigator.language;
    return browserLanguage?.toLowerCase().startsWith('pt') ? 'pt-BR' : 'en-US';
  }
}
