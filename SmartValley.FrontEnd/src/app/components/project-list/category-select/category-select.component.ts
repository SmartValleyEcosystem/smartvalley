import {Component, OnInit} from '@angular/core';
import {TranslateService} from '@ngx-translate/core';
import {DictionariesService} from '../../../services/common/dictionaries.service';
import {CategoryEnum} from '../../../services/common/category.enum';


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
  public categories: EnumItem<CategoryEnum>[];

  constructor(private dictionariesService: DictionariesService,
              private translateService: TranslateService) {
  }

  public async ngOnInit() {
    this.isCategoryListHovered = false;
    this.isSearchInputInFocus = false;
    this.selectedCategory = '';
    this.hideCategoryList();
    this.categories = this.dictionariesService.categories;
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
