import {InjectionToken} from "@angular/core";
export const BASE_PATH = new InjectionToken<string>('basePath');
export const CAPTCHAKEY = new InjectionToken<string>('captchaKey');

export const COLLECTION_FORMATS = {
  'csv': ',',
  'tsv': '   ',
  'ssv': ' ',
  'pipes': '|'
};
