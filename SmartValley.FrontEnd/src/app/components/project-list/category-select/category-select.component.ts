import {Component, OnInit} from '@angular/core';
import {Category} from '../../../services/common/category';
import {TranslateService} from '@ngx-translate/core';
import {CommonService} from '../../../services/common/common.service';


@Component({
  selector: 'app-category-select',
  templateUrl: './category-select.component.html',
  styleUrls: ['./category-select.component.css']
})
export class CategorySelectComponent implements OnInit {

  public isAutocompleteHidden: boolean;
  public isCategoryListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public selectedCategory: string;
  public selectedCategoryId: number;
  public categories: Category[];

  constructor(
    private commonService: CommonService,
    private translateService: TranslateService
  ) { }

  public async ngOnInit() {
    this.isCategoryListHovered = false;
    this.isSearchInputInFocus = false;
    this.selectedCategory = '';
    this.hideCategoryList();
    this.categories = this.commonService.categories;
  }

  public showCategoryList() {
    this.isAutocompleteHidden = false;
    this.squareInput = true;
  }

  public hideCategoryList() {
    if (this.isCategoryListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isAutocompleteHidden = true;
    this.squareInput = false;
  }

  public CategoryListHoverStatusSwitch(status: boolean) {
    this.isCategoryListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public selectCategory(id) {
    this.selectedCategory = this.translateService.instant('Categories.' + id);
    this.selectedCategoryId = id;
    this.isAutocompleteHidden = true;
  }

  public onCategoryInputChange() {
    this.selectedCategory = '';
    this.selectedCategoryId = undefined;
  }
}
