import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {isNullOrUndefined} from 'util';
import {Subscription} from 'rxjs/Subscription';
import {ProjectCardType} from '../../services/project-card-type';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectCardData} from '../common/project-card/project-card-data';
import {UserContext} from '../../services/authentication/user-context';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements OnDestroy, OnInit {
  public ProjectCardType = ProjectCardType;
  public projects: Array<ProjectCardData> = [];
  public selectedTabIndex: number;

  @ViewChild('projectsTabSet')
  private projectsTabSet: NgbTabset;
  private accountChangedSubscription: Subscription;

  constructor(private projectApiClient: ProjectApiClient,
              private userContext: UserContext) {
    this.accountChangedSubscription = this.userContext.userContextChanged.subscribe(
      () => this.reloadProjectsForScoringAsync());
  }

  async ngOnInit(): Promise<void> {
    await this.reloadProjectsForScoringAsync();
  }

  public async onExpertiseTabIndexChanged(index: number) {
    this.selectedTabIndex = index;
    await this.reloadProjectsForScoringAsync();
  }

  public ngOnDestroy(): void {
    if (!isNullOrUndefined(this.accountChangedSubscription) && !this.accountChangedSubscription.closed) {
      this.accountChangedSubscription.unsubscribe();
    }
  }

  private async reloadProjectsForScoringAsync(): Promise<void> {
    const expertiseArea = this.getExpertiseAreaByIndex(this.selectedTabIndex);
    const response = await this.projectApiClient.getForScoringAsync(expertiseArea);
    this.projects = response.items.map(p => ProjectCardData.fromProjectResponse(p, expertiseArea));
  }

  private getExpertiseAreaByIndex(index: number): ExpertiseArea {
    switch (index) {
      case 1 :
        return ExpertiseArea.Lawyer;
      case 2 :
        return ExpertiseArea.Analyst;
      case 3 :
        return ExpertiseArea.TechnicalExpert;
      default:
        return ExpertiseArea.HR;
    }
  }
}
