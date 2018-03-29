import {Component, OnInit, ViewChild} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ScoredProject} from '../../api/expert/scored-project';
import {Paths} from '../../paths';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {Router} from '@angular/router';
import {CountryAutocompleteComponent} from './country-autocomplete/country-autocomplete.component';
import {CategorySelectComponent} from './category-select/category-select.component';
import {ProjectsOrderBy} from '../../api/application/projects-order-by.enum';
import {SortDirection} from '../../api/sort-direction.enum';
import {ProjectResponse} from '../../api/project/project-response';
import {ProjectQuery} from '../../api/project/project-query';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {

  public ASC: SortDirection = SortDirection.Ascending;
  public DESC: SortDirection = SortDirection.Descending;
  public projects: ScoredProject[];
  public scoringRatingFrom: number;
  public scoringRatingTo: number;
  public sortedBy: ProjectsOrderBy;
  public sortDirection: SortDirection;
  public projectSearch: string;
  public projectOnPageCount = 10;
  public totalProjects: number;
  @ViewChild(CountryAutocompleteComponent) country: CountryAutocompleteComponent;
  @ViewChild(CategorySelectComponent) category: CategorySelectComponent;

  constructor(private router: Router,
              private expertApiClient: ExpertApiClient,
              private projectApiClient: ProjectApiClient) {
  }

  async ngOnInit() {
    this.sortDirection = this.ASC;
    this.sortedBy = ProjectsOrderBy.ScoringEndDate;
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
    this.projectSearch = '';

    await this.updateProjectsAsync(0);
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

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Report + '/' + id]).toString()
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
      countryCode: this.country.selectedCountryCode,
      categoryType: this.category.selectedCategoryId,
      orderBy: this.sortedBy,
      direction: this.sortDirection
    });
    this.projects = projectsResponse.items.map(p => this.createScoredProject(p));
    this.totalProjects = projectsResponse.totalCount;
  }

  public async clearFilters() {
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
    this.country.selectedCountry = '';
    this.country.selectedCountryCode = '';
    this.category.selectedCategory = '';
    this.category.selectedCategoryId = null;
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
