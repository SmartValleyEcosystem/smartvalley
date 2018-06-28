import {Pipe, PipeTransform} from '@angular/core';
import BigNumber from 'bignumber.js';
import {isNullOrUndefined} from 'util';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

  transform(val: BigNumber | number): string {
    if (typeof val === 'number') {
        val = new BigNumber(val);
    }
    if (!isNullOrUndefined(val)) {
      return val.toFormat(3, 3);
    }
    return '';
  }
}
