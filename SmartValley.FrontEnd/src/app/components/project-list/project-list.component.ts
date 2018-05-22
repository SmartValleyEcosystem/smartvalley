import {Component, OnInit, ViewChild} from '@angular/core';
import {ScoredProject} from '../../api/expert/scored-project';
import {Paths} from '../../paths';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Router} from '@angular/router';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {SortDirection} from '../../api/sort-direction.enum';
import {ProjectResponse} from '../../api/project/project-response';
import {ProjectQuery} from '../../api/project/project-query';
import {SelectItem} from 'primeng/api';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {SelectComponent} from '../select/select.component';
import {AutocompleteComponent} from '../autocomplete/autocomplete.component';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.scss']
})
export class ProjectListComponent implements OnInit {

  public ASC: SortDirection = SortDirection.Ascending;
  public DESC: SortDirection = SortDirection.Descending;
  public projects: ScoredProject[] = [];
  public checkProjects: boolean;
  public countries: SelectItem[] = [];
  public categories: SelectItem[] = [];
  public scoringRatingFrom?: number;
  public scoringRatingTo?: number;
  public sortedBy: ProjectsOrderBy;
  public sortDirection: SortDirection;
  public projectSearch: string;
  public projectOnPageCount = 10;
  public totalProjects: number;
  public selectedCountryCode: string;
  public isFormValid = true;

  @ViewChild(SelectComponent) category: SelectComponent;
  @ViewChild(AutocompleteComponent) country: AutocompleteComponent;

  constructor(private router: Router,
              private dictionariesService: DictionariesService,
              private projectApiClient: ProjectApiClient) {
    this.countries = this.dictionariesService.countries.map(i => <SelectItem>{
      label: i.name,
      value: i.code
    });

    this.categories = this.dictionariesService.categories.map(i => <SelectItem>{
      label: i.value,
      value: i.id
    });
  }

  async ngOnInit() {
    this.sortDirection = this.DESC;
    this.sortedBy = ProjectsOrderBy.ScoringEndDate;
    this.projectSearch = '';

    await this.updateProjectsAsync(0);
  }

  public selectedCountry(countryCode: string) {
    this.selectedCountryCode = countryCode;
  }

  private createScoredProject(response: ProjectResponse): ScoredProject {
    return <ScoredProject> {
      id: response.id,
      address: response.scoring ? response.scoring.contractAddress : '',
      category: response.category,
      country: response.country,
      description: response.description,
      name: response.name,
      score: response.scoring ? response.scoring.score : '',
      scoringEndDate: response.scoring ? response.scoring.scoringEndDate : ''
    };
  }

  public navigateToProject(id) {
    this.router.navigate([Paths.Project + '/' + id]);
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Project + '/' + id]).toString()
    );
  }

  public async submitFilters() {
    const validate = this.checkScoring();
    if (validate) {
      await this.updateProjectsAsync(0);
    }
  }

  public async updateProjectsAsync(page: number) {
    const projectsResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: page * this.projectOnPageCount,
      count: this.projectOnPageCount,
      onlyScored: false,
      searchString: this.projectSearch,
      minimumScore: this.scoringRatingFrom,
      maximumScore: this.scoringRatingTo,
      countryCode: this.selectedCountryCode,
      categoryType: this.category.selectedItemValue,
      orderBy: this.sortedBy,
      direction: this.sortDirection
    });
    this.projects = projectsResponse.items.map(p => this.createScoredProject(p));
    this.totalProjects = projectsResponse.totalCount;
    this.checkProjects = true;
  }

  public async clearFilters() {
    this.scoringRatingFrom = null;
    this.scoringRatingTo = null;
    this.selectedCountryCode = '';
    this.category.resetSelection();
    this.country.resetSelection();
    this.projectSearch = '';

    await this.updateProjectsAsync(0);
  }

  public async changePage(event) {
    await this.updateProjectsAsync(event.page);
  }

  public async sortByName() {
    return this.sortBy(ProjectsOrderBy.Name);
  }

  public async sortByDate() {
    return this.sortBy(ProjectsOrderBy.ScoringEndDate);
  }

  public async sortByRating() {
    return this.sortBy(ProjectsOrderBy.ScoringRating);
  }

  public async sortBy(orderBy: ProjectsOrderBy): Promise<void> {
    if (this.sortedBy === orderBy) {
      if (this.sortDirection === SortDirection.Descending) {
        this.sortDirection = SortDirection.Ascending;
      } else {
        this.sortDirection = SortDirection.Descending;
      }
    }
    this.sortedBy = orderBy;
    await this.updateProjectsAsync(0);
  }

  public isSortableField(sortField: string): boolean {
    return this.sortedBy === ProjectsOrderBy[sortField];
  }

  public isSortableDirection(direction: SortDirection): boolean {
    return this.sortDirection === direction;
  }

  private coloredText(text: string) {
    if (this.projectSearch === '') {
      return text;
    }

    const lowerText: string = text.toLowerCase();
    const lowerSearch = this.projectSearch.toLowerCase();

    let coloredString = '';
    const words = lowerText.split(lowerSearch);
    for (let i = 0; i < words.length; i++) {
      let startIndex = 0;
      if (words[i] !== '') {
        startIndex = lowerText.indexOf(words[i]) + words[i].length;
      } else {
        if (words.length > 1 && i > 0) {
          startIndex = lowerText.indexOf(words[i - 1]) + words[i - 1].length;
        }
      }
      const lastIndex: number = startIndex + lowerSearch.length;
      const word = text.substring(startIndex, lastIndex);

      if (word === '') {
        continue;
      }

      const replacedWord = '<span style=\"background-color: #ffd038; color: black;\">' + word + '</span>';
      coloredString = text.replace(word, replacedWord);
    }

    if (words.every(i => i === '')) {
      const word = text.substring(0, lowerSearch.length);

      const replacedWord = '<span style=\"background-color: #ffd038; color: black;\">' + word + '</span>';
      coloredString = text.replace(word, replacedWord);
    }

    return coloredString === '' ? text : coloredString;
  }

  public checkScoring() {
    this.isFormValid = true;
    if (this.scoringRatingFrom > 100 || this.scoringRatingFrom < 0) {
      this.isFormValid = false;
    }
    if (this.scoringRatingTo > 100 || this.scoringRatingTo < 0) {
      this.isFormValid = false;
    }
    if (this.scoringRatingFrom > this.scoringRatingTo) {
      this.isFormValid = false;
    }
    return this.isFormValid;
  }
}
