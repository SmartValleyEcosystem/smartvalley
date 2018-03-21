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
import {ProjectCategory} from '../../api/application/project-category.enum';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {

  public ASC: SortDirection = SortDirection.Ascending;
  public DESC: SortDirection  = SortDirection.Descending;
  public scoredProjects: ScoredProject[];
  public scoringRatingFrom: number;
  public scoringRatingTo: number;
  public sortedBy: ProjectsOrderBy;
  public sortDirection: SortDirection;
  public projectSearch: string;
  public projectOnPageCount = 10;
  public totalProjects: number;
  public currentPage = 0;
  @ViewChild(CountryAutocompleteComponent) country: CountryAutocompleteComponent;
  @ViewChild(CategorySelectComponent) category: CategorySelectComponent;

  constructor(
    private router: Router,
    private expertApiClient: ExpertApiClient,
    private projectApiClient: ProjectApiClient) { }

  async ngOnInit() {
    this.sortDirection = this.ASC;
    this.sortedBy  = ProjectsOrderBy.None;
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
    this.projectSearch = '';

    let projectsResponse = await this.getFilteredProjectsAsync();
    this.scoredProjects = projectsResponse.items;
    this.totalProjects = projectsResponse.totalCount;
  }

  public async getFilteredProjectsAsync() {
    return await this.projectApiClient.getFilteredProjectsAsync({
      offset: this.currentPage * this.projectOnPageCount,
      count: this.projectOnPageCount,
      searchString: this.projectSearch,
      minimumScore: this.scoringRatingFrom,
      maximumScore: this.scoringRatingTo,
      countryCode: this.country.selectedCountryCode,
      categoryType: this.category.selectedCategoryId,
      orderBy: this.sortedBy,
      direction: this.sortDirection
    });
  }

  public getProjectLink(id) {
    return decodeURIComponent(
      this.router.createUrlTree([Paths.Report + '/' + id]).toString()
    );
  }

  public async submitFilters() {
    this.updateProjects();
  }

  public async updateProjects() {
    let projectsResponse = await this.getFilteredProjectsAsync();
    return this.scoredProjects = projectsResponse.items;
  }

  public clearFilters() {
    this.scoringRatingFrom = 0;
    this.scoringRatingTo = 100;
    this.country.selectedCountry = '';
    this.country.selectedCountryCode = '';
    this.category.selectedCategory = '';
    this.projectSearch = '';
  }

  public getCategoryName(id): string {
    return ProjectCategory[id];
  }

  public async changePage(event) {
    this.currentPage = event.page;

    this.updateProjects();
  }

  public async sortProjectsBy(sortField: string) {
    if ( this.sortedBy === ProjectsOrderBy[sortField] ) {
      if (this.sortDirection === SortDirection.Descending ) {
        this.sortDirection = SortDirection.Ascending;
      } else {
        this.sortDirection = SortDirection.Descending ;
      }
    }
    this.sortedBy = ProjectsOrderBy[sortField];

    this.updateProjects();
  }

  public isSortableField(sortField: string): boolean {
    return this.sortedBy === ProjectsOrderBy[sortField];
  }

  public isSortableDirection(direction: SortDirection): boolean {
    return this.sortDirection === direction;
  }

}
