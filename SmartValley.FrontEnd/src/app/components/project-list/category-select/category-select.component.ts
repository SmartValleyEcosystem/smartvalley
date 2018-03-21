import {Component, Input, OnInit} from '@angular/core';
import {ApplicationApiClient} from '../../../api/application/application-api.client';
import {CategoryResponse} from '../../../api/application/category-response';
import {ProjectCategory} from '../../../api/application/project-category.enum';


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
  public categories: ProjectCategory[];
  public categoriesEnum = ProjectCategory;

  constructor(
    private applicationApiClient: ApplicationApiClient
  ) { }

  public async ngOnInit() {
    this.isCategoryListHovered = false;
    this.isSearchInputInFocus = false;
    this.selectedCategory = '';
    this.hideCategoryList();
    let categoriesResponse = await this.applicationApiClient.getCategoriesAsync();
    this.categories = categoriesResponse.items.map( cat => cat.id);
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
    this.selectedCategory = this.categoriesEnum[id];
    this.selectedCategoryId = id;
    this.isAutocompleteHidden = true;
  }

  public onCategoryInputChange() {
    this.selectedCategory = '';
    this.selectedCategoryId = undefined;
  }
}
