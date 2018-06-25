import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

    transform(val: number, decimal = 3, floor = false, locale = 'fr'): string {
        if (val !== undefined && val !== null) {
            val = val / Math.pow(10, decimal);
            return floor ? Math.floor(val).toLocaleString(locale) : val.toLocaleString(locale);
        }
        return '';
    }

}
