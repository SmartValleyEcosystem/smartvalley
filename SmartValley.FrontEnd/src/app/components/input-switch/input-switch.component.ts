import {Component, Input, OnInit, forwardRef, OnChanges} from '@angular/core';
import {FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';

@Component({
  selector: 'app-input-switch',
  templateUrl: './input-switch.component.html',
  styleUrls: ['./input-switch.component.css'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => InputSwitchComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => InputSwitchComponent), multi: true }
  ]
})
export class InputSwitchComponent implements ControlValueAccessor, OnChanges {

  public propagateChange: any = () => {};
  public validateFn: any = () => {};
  @Input('switcherValue') _switcherValue = null;
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() labelOn?: string;
  @Input() labelOff?: string;
  @Input() styleClass?: string;

  get switcherValue() {
    return this._switcherValue;
  }

  set switcherValue(val: boolean|null) {
    this._switcherValue = val;
    this.propagateChange(val);
  }

  public changeValue(value: boolean|null) {
    this.switcherValue = value;
  }

  ngOnChanges(inputs) {
    this.propagateChange(this.switcherValue);
  }
  writeValue(value) {
    if (value) {
      this.switcherValue = value;
    }
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }

  registerOnTouched() {}

  validate(c: FormControl) {
    return this.validateFn(c);
  }

}
