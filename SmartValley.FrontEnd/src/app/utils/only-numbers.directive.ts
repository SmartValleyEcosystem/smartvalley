import {Directive, ElementRef, HostListener, Input} from '@angular/core';

@Directive({
    selector: '[appOnlyNumbersByDecimal]'
})
export class OnlyNumbersByDecimalDirective {
    private regex: RegExp = new RegExp(/^-?[0-9]+(\.[0-9]*){0,1}$/g);
    private decimalRegex: RegExp = new RegExp(/\.(.*)/);
    private specialKeys: Array<string> = [ 'Backspace', 'Tab', 'End', 'Home', '.', 'ArrowLeft', 'ArrowRight', 'Delete'];

    @Input() decimal: number;

    constructor(private el: ElementRef) {
    }

    @HostListener('keydown', [ '$event' ])

    onKeyDown(event: KeyboardEvent) {
      if (!this.decimal) {
        this.decimal = 1;
      }

      let current: string = this.el.nativeElement.value;
      let next: string = current.concat(event.key);

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
