import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {Constants} from '../../constants';
import {FormControl} from '@angular/forms';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
import {ProjectQuery} from '../../api/project/project-query';
import {ProjectResponse} from '../../api/project/project-response';
import {ColorHelper} from '../../utils/color-helper';
import {Category} from '../../services/common/category';
@Component({
  selector: 'app-search-with-autocomplete',
  templateUrl: './search-with-autocomplete.component.html',
  styleUrls: ['./search-with-autocomplete.component.scss']
})
export class SearchWithAutocompleteComponent implements OnInit {

  public projects: ProjectResponse[] = [];
  public isAutocompleteHidden: boolean;
  public isProjectListHovered: boolean;
  public isSearchInputInFocus: boolean;
  public squareInput = false;
  public inputSearch: FormControl;

  constructor(private projectApiClient: ProjectApiClient,
              private router: Router) { }

  public async ngOnInit() {
    this.isProjectListHovered = false;
    this.isSearchInputInFocus = false;
    this.hideProjectList();
    this.inputSearch = new FormControl('');

    this.inputSearch.valueChanges
      .debounceTime(500)
      .subscribe(newValue => this.searchRequestAsync(newValue));
  }

  public showProjectList() {
    this.isAutocompleteHidden = false;
    this.squareInput = true;
  }

  public hideProjectList() {
    if (this.isProjectListHovered) {
      return;
    }
    if (this.isSearchInputInFocus) {
      return;
    }
    this.isAutocompleteHidden = true;
    this.squareInput = false;
  }

  public projectListHoverStatusSwitch(status: boolean) {
    this.isProjectListHovered = status;
  }

  public searchInputFocusStatusSwitch(status: boolean) {
    this.isSearchInputInFocus = status;
  }

  public async searchRequestAsync(search) {
    const searchResult = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: 0,
      count: 10,
      searchString: search,
      onlyScored: false,
    });
    this.projects = searchResult.items;
  }

  public navigateToProject(id) {
    this.router.navigate([Paths.Project + '/' + id], {queryParams: {tab: Constants.ReportFormTab}});
  }

  public submit() {
    this.router.navigate([Paths.ProjectList + '/' + this.inputSearch.value]);
  }

  public markedText(text: string) {
    return ColorHelper.coloredText(text, this.inputSearch.value);
  }

  public getCategoryByIndex(id: number): string {
    return Category[id];
}

}
