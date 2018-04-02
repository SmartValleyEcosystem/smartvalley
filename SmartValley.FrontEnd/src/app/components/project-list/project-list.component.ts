import {Component, OnInit, ViewChild} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
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
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {

  public ASC: SortDirection = SortDirection.Ascending;
  public DESC: SortDirection = SortDirection.Descending;
  public projects: ScoredProject[] = [];
  public countries: SelectItem[] = [];
  public categories: SelectItem[] = [];
  public scoringRatingFrom: number;
  public scoringRatingTo: number;
  public sortedBy: ProjectsOrderBy;
  public sortDirection: SortDirection;
  public projectSearch: string;
  public projectOnPageCount = 10;
  public totalProjects: number;
  public selectedCountryCode: string;

  @ViewChild(SelectComponent) category: SelectComponent;
  @ViewChild(AutocompleteComponent) country: AutocompleteComponent;

  constructor(private router: Router,
              private expertApiClient: ExpertApiClient,
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
    this.sortDirection = this.ASC;
    this.sortedBy = ProjectsOrderBy.ScoringEndDate;
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
    this.projectSearch = '';

    await this.updateProjectsAsync(0);
  }

  public selectedCountry(countryCode: string) {
    this.selectedCountryCode = countryCode;
  }

  private createScoredProject(response: ProjectResponse): ScoredProject {
    return <ScoredProject> {
      id: response.id,
      address: response.address,
      category: response.category,
      country: response.country,
      description: response.description,
      name: response.name,
      score: response.score,
      scoringEndDate: response.scoringEndDate
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
    await this.updateProjectsAsync(0);
  }

  public async updateProjectsAsync(page: number) {
    const projectsResponse = await this.projectApiClient.queryProjectsAsync(<ProjectQuery>{
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
  }

  public async clearFilters() {
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
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
}
