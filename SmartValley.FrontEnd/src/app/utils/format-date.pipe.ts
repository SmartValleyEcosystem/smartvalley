import {Pipe, PipeTransform} from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'formatDate'
})
export class FormatDatePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value instanceof Date) {
      return moment(value).format('MMMM D, Y');
    }
    return value;
  }
}
