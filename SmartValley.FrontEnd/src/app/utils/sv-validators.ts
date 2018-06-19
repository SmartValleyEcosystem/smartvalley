import {AbstractControl, ValidationErrors, ValidatorFn, Validators} from '@angular/forms';
import {isNullOrUndefined} from 'util';

export class SVValidators extends Validators {
  static checkFutureDate(control: AbstractControl) {
    if (isNullOrUndefined(control.value) || control.value === '') {
      return null;
    }
    const currentDate = new Date(Date.now());
    const valueDate = (control.value as Date);
    return valueDate > currentDate ? null : {dateIsNotFuture: true};
  }
}
