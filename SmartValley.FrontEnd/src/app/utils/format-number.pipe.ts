import {Pipe, PipeTransform} from '@angular/core';
import BigNumber from 'bignumber.js';
import {isNullOrUndefined} from 'util';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

  transform(val: BigNumber): string {
    if (!isNullOrUndefined(val)) {
      return val.toFormat();
    }
    return '';
  }
}
