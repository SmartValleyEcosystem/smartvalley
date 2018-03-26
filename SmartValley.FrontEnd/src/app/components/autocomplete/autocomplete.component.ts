import {Component, Input, OnInit} from '@angular/core';
import {SelectItem} from 'primeng/api';
import {FormGroup} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.css']
})
export class AutocompleteComponent implements OnInit {

  public items: SelectItem[] = [];
  public isAutocompleteHidden: boolean;
  public isAreaListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public selectedItemLabel: string;
  public selectedItemValue: string | number;
  @Input() placeholder: string;
  @Input() allItems: SelectItem[] = [];
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() elementClass: string;
  @Input() isNeedToTranslate? = false;
  @Input() form?: FormGroup;

  constructor( private translateService: TranslateService ) { }

  public ngOnInit() {
    this.selectedItemLabel = '';
    this.selectedItemValue = '';

    this.isAreaListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideItemsList();

    this.items = this.allItems;
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

  public selectItem(label, value) {
    this.selectedItemLabel = label;
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

}
