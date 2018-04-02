import {Component, Input, forwardRef, OnChanges, ElementRef, OnInit} from '@angular/core';
import {FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => SelectComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => SelectComponent), multi: true }
  ]
})
export class SelectComponent implements ControlValueAccessor, OnChanges, OnInit {

  public isSelectHidden: boolean;
  public isSelectListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public selectedInput = false;
  public selectedItemLabel: string;

  @Input('selectedItemValue') _selectedItemValue: string|number|null = null;
  @Input() placeholder: string;
  @Input() items: SelectItem[];
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() form?: FormGroup;
  @Input() isNeedToTranslate? = false;
  @Input() defaultValue?: string|number;

  constructor( private translateService: TranslateService,
               private nativeElement: ElementRef ) {}

  public propagateChange: any = () => {};

  public validateFn: any = () => {};

  get selectedItemValue() {
    return this._selectedItemValue;
  }

  set selectedItemValue(value: string|number) {
    this._selectedItemValue = value;
    this.propagateChange(value);
  }

  public ngOnChanges(inputs) {
    this.propagateChange(this.selectedItemValue);
  }

  public writeValue(value) {
    if (value) {
      this.selectedItemValue = value;
    }
  }

  public ngOnInit() {
    this.selectedItemLabel = '';
    this.selectedItemValue = '';
    this.isSelectListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideSelectList();
    if (this.defaultValue) {
      this.setDefaultValue();
    }
  }

  public setDefaultValue() {
    for (const item of this.items) {
      if (item.value === this.defaultValue) {
        this.selectedItemLabel = this.isNeedToTranslate ? this.translateService.instant(item.label) : item.label;
      }
    }
    this.selectedItemValue = this.defaultValue;
  }

  public registerOnTouched() {}

  public validate(c: FormControl) {
    return this.validateFn(c);
  }

  public showSelectList() {
    this.isSelectHidden = false;
    this.selectedInput = true;
  }

  public hideSelectList() {
    if (this.isSelectListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isSelectHidden = true;
    this.selectedInput = false;
  }

  public selectListHoverStatusSwitch(status: boolean) {
    this.isSelectListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public selectItem(label: string, value: string|number|null) {
    this.selectedItemLabel = label;
    if (this.isNeedToTranslate) {
      this.selectedItemLabel = this.translateService.instant(label);
    }
    this.selectedItemValue = value;
    this.isSelectHidden = true;
  }

  public onSelectInputChange() {
    this.selectedItemValue = '';
  }

  public resetSelection() {
    this.selectedItemValue = '';
    this.selectedItemLabel = '';
    this.isSelectHidden = true;
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }

}

