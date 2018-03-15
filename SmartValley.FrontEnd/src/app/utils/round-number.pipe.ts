import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'roundNumberPipe'
})
export class roundNumberPipe implements PipeTransform {

  transform (input: string|number) {
    return Math.floor(+input);
  }
}
