import { Pipe, PipeTransform } from '@angular/core';
import BigNumber from 'bignumber.js';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

    transform(val: BigNumber, decimal = 3, floor = false): string {
        if (val !== undefined && val !== null) {
          val = val.dividedBy(Math.pow(10, decimal));
          const formatOptions = {
            minimumIntegerDigits: 3,
            minimumFractionDigits: 3,
            maximumFractionDigits: 3,
            minimumSignificantDigits: 3,
            maximumSignificantDigits: 3
          };
          return floor ? Math.floor(val.toNumber()).toLocaleString(navigator.language, formatOptions) : val.toNumber().toLocaleString(navigator.language, formatOptions);
        }
        return '';
    }

}
