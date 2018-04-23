import {Pipe, PipeTransform} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';

@Pipe({
  name: 'translateBinaryAnswer'
})
export class TranslateBinaryAnswer implements PipeTransform {

  constructor (private translateService: TranslateService) {
  }

  transform(value: any): any {
    if (value === "0" || value === "1") {
      return value === "0" ? this.translateService.instant('EditScoringApplication.No') : this.translateService.instant('EditScoringApplication.Yes');
    }
    return value;
  }
}
