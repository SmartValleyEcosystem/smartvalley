import {Component, Input, Output, forwardRef, OnChanges, EventEmitter, ElementRef, OnInit} from '@angular/core';
import {SelectItem} from 'primeng/api';
import {FormGroup, FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, NG_VALIDATORS} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';


@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.scss'],
  providers: [
    {provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => AutocompleteComponent), multi: true},
    {provide: NG_VALIDATORS, useExisting: forwardRef(() => AutocompleteComponent), multi: true}
  ]
})
export class AutocompleteComponent implements ControlValueAccessor, OnChanges, OnInit {

  public items: SelectItem[] = [];
  public isAutocompleteHidden: boolean;
  public isAreaListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public selectedItemLabel: string;

  @Input('selectedItemValue') _selectedItemValue: string | number | null = null;
  @Input() placeholder: string;
  @Input() allItems: SelectItem[] = [];
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() elementClass: string;
  @Input() isNeedToTranslate? = false;
  @Input() form?: FormGroup;
  @Input() defaultValue?: string | number;
  @Output() onSelect: EventEmitter<string | number | null> = new EventEmitter<string | number | null>();

  constructor(private translateService: TranslateService,
              private nativeElement: ElementRef) {
  }

  public ngOnInit() {
    this.selectedItemLabel = '';
    this.selectedItemValue = '';

    this.isAreaListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideItemsList();

    this.items = this.allItems;
    if (this.defaultValue) {
      this.setDefaultValue(this.defaultValue);
    }
  }

  public setDefaultValue(value) {
    for (const item of this.allItems) {
      if (item.value === value) {
        this.selectedItemLabel = this.isNeedToTranslate ? this.translateService.instant(item.label) : item.label;
      }
    }
    this.selectedItemValue = value;
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

  public selectItem(label: string, value: string | number | null) {
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
    this.items = this.allItems.filter(c => c.label.toLowerCase().includes(event.target.value.toLowerCase()));
  }

  public resetSelection() {
    this.selectedItemValue = '';
    this.selectedItemLabel = '';
    this.isAutocompleteHidden = true;
  }

  public propagateChange: any = () => {}

  public validateFn: any = () => {}

  get selectedItemValue(): string | number | null {
    return this._selectedItemValue;
  }

  set selectedItemValue(value: string | number) {
    this._selectedItemValue = value;
    this.propagateChange(value);
    this.onSelect.emit(value);
  }

  public ngOnChanges(inputs) {
    this.propagateChange(this.selectedItemValue);
  }

  public writeValue(value: string | number | null) {
    if (value) {
      this.selectedItemValue = value;
    }
    this.setDefaultValue(this.selectedItemValue);
  }

  public registerOnTouched() {
  }

  public validate(c: FormControl) {
    return this.validateFn(c);
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }
}
