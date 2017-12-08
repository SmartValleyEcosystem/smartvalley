import {TranslateLoader} from '@ngx-translate/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/observable/forkJoin';

export class MultiTranslateLoader implements TranslateLoader {

  constructor(private resources: Array<string>) {
  }

  public getTranslation(lang: string): any {
    return Observable.forkJoin(this.resources.map(filePath => {
      return System.import(`../../../app/${filePath}-${lang}.json`);
    })).map(response => {
      return response.reduce((a, b) => {
        return Object.assign(a, b);
      });
    });
  }
}
