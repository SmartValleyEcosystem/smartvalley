import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'roundNumberPipe'
})
export class RoundNumberPipe implements PipeTransform {

  transform(input: number, floatNumber = 0) {
    if (floatNumber) {
      return input.toFixed(floatNumber);
    }
    return Math.floor(+input);
  }
}
