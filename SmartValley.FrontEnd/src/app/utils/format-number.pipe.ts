import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatNumber'
})
export class FormatNumberPipe implements PipeTransform {

    transform(val: number, locale = 'fr'): string {
        if (val !== undefined && val !== null) {
            return val.toLocaleString(locale);
        }
        return '';
    }

}
