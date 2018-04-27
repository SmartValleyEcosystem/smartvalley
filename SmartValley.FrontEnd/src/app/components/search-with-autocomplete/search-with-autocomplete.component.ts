import {Component, OnInit} from '@angular/core';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {SearchProjectResponse} from '../../api/project/search-projects-response';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {Constants} from '../../constants';
import {FormControl} from '@angular/forms';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/throttleTime';
import 'rxjs/add/observable/fromEvent';
@Component({
  selector: 'app-search-with-autocomplete',
  templateUrl: './search-with-autocomplete.component.html',
  styleUrls: ['./search-with-autocomplete.component.scss']
})
export class SearchWithAutocompleteComponent implements OnInit {

  public projects: SearchProjectResponse[] = [];
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
    const searchResult = await this.projectApiClient.getProjectsBySearchStringAsync(search);
    this.projects = searchResult.items;
  }

  public navigateToProject(id) {
    this.router.navigate([Paths.Project + '/' + id], {queryParams: {tab: Constants.ReportFormTab}});
  }

  public submit() {
    this.router.navigate([Paths.ProjectList], {queryParams: {search: this.inputSearch.value}});
  }

  private coloredText(text: string) {
    if (this.inputSearch.value === '') {
      return text;
    }

    const lowerText: string = text.toLowerCase();
    const lowerSearch = this.inputSearch.value.toLowerCase();

    let coloredString = '';
    const words = lowerText.split(lowerSearch);
    for (let i = 0; i < words.length; i++) {
      if (words[i] === '') {
        continue;
      }
      const startIndex: number = lowerText.indexOf(words[i]) - lowerSearch.length;
      const lastIndex: number = startIndex + lowerSearch.length;
      const word = text.substring(startIndex, lastIndex);

      if (word === '') {
        continue;
      }

      const replacedWord = '<span style=\"background-color: #ffd038; color: black;\">' + word + '</span>';
      coloredString = text.replace(word, replacedWord);
    }
    return coloredString === '' ? text : coloredString;
  }

}
