import {Component, Input, OnInit, forwardRef, OnChanges} from '@angular/core';
import {SelectItem} from 'primeng/api';
import {FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';


@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.css'],
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => AutocompleteComponent), multi: true },
    { provide: NG_VALIDATORS, useExisting: forwardRef(() => AutocompleteComponent), multi: true }
  ]
})
export class AutocompleteComponent implements ControlValueAccessor, OnChanges {

  public propagateChange: any = () => {};
  public validateFn: any = () => {};
  public items: SelectItem[] = [];
  public isAutocompleteHidden: boolean;
  public isAreaListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public selectedItemLabel: string;
  @Input('selectedItemValue') _selectedItemValue: string|number|null = null;
  @Input() placeholder: string;
  @Input() allItems: SelectItem[] = [];
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() elementClass: string;
  @Input() isNeedToTranslate? = false;
  @Input() form?: FormGroup;
  @Input() defaultValue?: string|number;

  constructor( private translateService: TranslateService ) {}

  get selectedItemValue(): string|number|null {
    return this._selectedItemValue;
  }

  set selectedItemValue(value: string|number) {
    this._selectedItemValue = value;
    this.propagateChange(value);
  }

  public ngOnChanges(inputs) {
    this.propagateChange(this.selectedItemValue);
  }

  public writeValue(value: string|number|null) {
    if (value) {
      this.selectedItemValue = value;
    }
  }

  public ngOnInit() {
    this.selectedItemLabel = '';
    this.selectedItemValue = '';

    this.isAreaListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideItemsList();

    this.items = this.allItems;
    if (this.defaultValue) {
      this.setDefaultValue();
    }
  }

  public setDefaultValue() {
    for (const item of this.allItems) {
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

  public showItemsList() {
    this.isAutocompleteHidden = false;
    this.squareInput = true;
  }

  public hideItemsList() {
    if (this.isAreaListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isAutocompleteHidden = true;
    this.squareInput = false;
  }

  public selectListHoverStatusSwitch(status: boolean) {
    this.isAreaListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public selectItem(label: string, value: string|number|null) {
    this.selectedItemLabel = label;
    this.selectedItemValue = value;
    if (this.isNeedToTranslate) {
      this.selectedItemLabel = this.translateService.instant(label);
    }
    this.selectedItemValue = value;
    this.isAutocompleteHidden = true;
  }

  public onAutocompleteInputChange(event) {
    this.selectedItemValue = '';
    this.items = this.allItems.filter( c => c.label.toLowerCase().includes(event.target.value.toLowerCase()));
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }
}
