import {Component, Input, OnInit, forwardRef, OnChanges, ElementRef} from '@angular/core';
import {FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS} from '@angular/forms';

@Component({
  selector: 'app-input-switch',
  templateUrl: './input-switch.component.html',
  styleUrls: ['./input-switch.component.css'],
  providers: [
    {provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => InputSwitchComponent), multi: true},
    {provide: NG_VALIDATORS, useExisting: forwardRef(() => InputSwitchComponent), multi: true}
  ]
})
export class InputSwitchComponent implements ControlValueAccessor, OnChanges, OnInit {

  public propagateChange: any = () => {}
  public validateFn: any = () => {}

  @Input('switcherValue') _switcherValue: number | null = null;
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() labelOn?: string;
  @Input() labelOff?: string;
  @Input() styleClass?: string;
  @Input() defaultValue?: number | string | null;

  constructor(private nativeElement: ElementRef) {
  }

  get switcherValue() {
    return this._switcherValue;
  }

  set switcherValue(val: number | null) {
    this._switcherValue = val;
    this.propagateChange(val);
  }

  public changeValue(value: number | null) {
    this.switcherValue = value;
  }

  public ngOnInit() {
    if (this.defaultValue) {
      this.setDefaultValue(this.defaultValue);
    }
  }

  ngOnChanges(inputs) {
    this.propagateChange(this.switcherValue);
  }

  writeValue(value) {
    if (value) {
      this.switcherValue = value;
    }
    this.setDefaultValue(value);
  }

  public setDefaultValue(value) {
    let preparedValue: number | null = null;
    if (value) {
      preparedValue = +value;
    }
    this.switcherValue = preparedValue;
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }

  registerOnTouched() {
  }

  validate(c: FormControl) {
    return this.validateFn(c);
  }
}
