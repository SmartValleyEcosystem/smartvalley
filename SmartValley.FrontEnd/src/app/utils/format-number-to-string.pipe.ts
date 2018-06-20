import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatNumberToString'
})
export class FormatNumberToStringPipe implements PipeTransform {

    transform(val: number, locale = 'fr'): string {
        if (val !== undefined && val !== null) {
            return val.toLocaleString(locale);
        }
        return '';
    }

}
