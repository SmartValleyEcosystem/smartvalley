import {Pipe, PipeTransform} from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'formatDate'
})
export class FormatDatePipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value instanceof Date || typeof value === 'string') {
      return moment(value).format('D MMMM Y');
    }
    return value;
  }
}
