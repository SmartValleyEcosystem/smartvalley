import {Pipe, PipeTransform} from '@angular/core';
import BigNumber from 'bignumber.js';
import {isNullOrUndefined} from 'util';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

  transform(val: BigNumber | number, showDecimalIfInteger = false ): string {
    if (typeof val === 'number') {
        val = new BigNumber(val);
    }
    if (!showDecimalIfInteger && val.decimalPlaces() === 0) {
      return val.toFormat(0, 0);
    }
    if (!isNullOrUndefined(val)) {
      return val.toFormat(3, 3);
    }
    return '';
  }
}
