import {Directive, ElementRef, HostListener, Input} from '@angular/core';
import {BigNumber} from 'bignumber.js';

@Directive({
    selector: '[appOnlyNumbersByDecimal]'
})
export class OnlyNumbersByDecimalDirective {
    private regex: RegExp = new RegExp(/^-?[0-9]+(\.[0-9]*){0,1}$/g);
    private decimalRegex: RegExp = new RegExp(/\.(.*)/);
    private specialKeys: Array<string> = [ 'Backspace', 'Tab', 'End', 'Home', 'ArrowLeft', 'ArrowRight', 'Delete'];

    @Input() decimal: number;
    @Input() maxValue: BigNumber;

    constructor(private el: ElementRef) {
    }

    @HostListener('keydown', [ '$event' ])

    onKeyDown(event: KeyboardEvent) {
      let current: string = this.el.nativeElement.value;
      let next: string = current.concat(event.key);

      if (this.specialKeys.indexOf(event.key) !== -1) {
        return;
      }

      if (next.match(/^\./)) {
        this.el.nativeElement.value = '0' + this.el.nativeElement.value;
        event.preventDefault();
      }

      if (((next || '').match(/\./g) || []).length > 1) {
        event.preventDefault();
      }

      if (next.match(this.decimalRegex)) {
        if (next.match(this.decimalRegex)[1].length > this.decimal) {
          event.preventDefault();
        }
      }

      if (next && !String(next).match(this.regex)) {
          event.preventDefault();
      }
    }
}
