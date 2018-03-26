import {Component, Input, OnInit} from '@angular/core';
import {SelectItem} from 'primeng/api';
import {FormGroup} from '@angular/forms';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css']
})
export class SelectComponent implements OnInit {

  public isAutocompleteHidden: boolean;
  public isSelectListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public selectedInput = false;
  public selectedItem: string;
  public selectedItemId: number;
  public itemsValues: SelectItem[];
  @Input() placeholder: string;
  @Input() items: SelectItem[];
  @Input() elementId: string;
  @Input() elementName: string;
  @Input() form?: FormGroup;
  @Input() isNeedToTranslate? = false;

  constructor( private translateService: TranslateService ) { }

  public async ngOnInit() {
    this.isSelectListHovered = false;
    this.isSearchInputInFocus = false;
    this.selectedItem = '';
    this.hideSelectList();
  }

  public showSelectList() {
    this.isAutocompleteHidden = false;
    this.selectedInput = true;
  }

  public hideSelectList() {
    if (this.isSelectListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isAutocompleteHidden = true;
    this.selectedInput = false;
  }

  public selectListHoverStatusSwitch(status: boolean) {
    this.isSelectListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public selectItem(label, value) {
    this.selectedItem = label;
    if (this.isNeedToTranslate) {
      this.selectedItem = this.translateService.instant(label);
    }
    this.selectedItemId = value;
    this.isAutocompleteHidden = true;
  }

  public onSelectInputChange() {
    this.selectedItem = '';
    this.selectedItemId = undefined;
  }

  public clearItems() {
    this.selectItem('', '');
  }

}
