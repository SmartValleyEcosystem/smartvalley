import {Pipe, PipeTransform} from '@angular/core';
import BigNumber from 'bignumber.js';
import {isNullOrUndefined} from 'util';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

  transform(val: BigNumber, decimalPlaces: number, subUnitPalces: number): string {
    if (!isNullOrUndefined(val)) {
      if (typeof val === 'number') {
        val = new BigNumber(val);
      }
      if (!isNullOrUndefined(subUnitPalces)) {
        val = val.shift(-subUnitPalces);
      }
      if (val.decimalPlaces() === 0) {
        return val.toFormat(0, 0);
      }
      if (!isNullOrUndefined(decimalPlaces)) {
        return val.toFormat(decimalPlaces, 3);
      }
      return val.toFormat(3, 3);
    }
    return '';
  }
}
