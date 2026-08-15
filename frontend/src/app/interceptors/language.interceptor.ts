import {inject} from '@angular/core';
import {HttpInterceptorFn} from '@angular/common/http';
import {LanguageService} from '../services/language.service';

export const languageInterceptor: HttpInterceptorFn = (req, next) => {
  const languageService = inject(LanguageService);

  const request = req.clone({
    params: req.params.set('lang', languageService.getLanguage())
  });

  return next(request);
};
