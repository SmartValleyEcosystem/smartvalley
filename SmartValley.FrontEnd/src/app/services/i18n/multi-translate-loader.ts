import {TranslateLoader} from '@ngx-translate/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';

export class MultiTranslateLoader implements TranslateLoader {

  constructor(public resources: { prefix: string, suffix: string }[]) {
  }

  /**
   * Gets the translations from the server
   * @param lang
   * @returns {any}
   */
  public getTranslation(lang: string): any {

    return Observable.forkJoin(this.resources.map(config => {
      return System.import('../../../assets/i18n/' + config.prefix + lang + '.json');
    })).map(response => {
      return response.reduce((a, b) => {
        return Object.assign(a, b);
      });
    });
  }
}
