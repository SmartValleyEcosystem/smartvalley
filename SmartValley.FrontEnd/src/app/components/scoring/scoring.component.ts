import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ProjectsForScoringRequest} from '../../api/scoring/projecs-for-scoring-request';
import {isNullOrUndefined} from 'util';
import {Subscription} from 'rxjs/Subscription';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements OnDestroy, OnInit {
  public projects: Array<Project> = [];
  public selectedTabIndex: number;

  @ViewChild('projectsTabSet')
  private projectsTabSet: NgbTabset;
  private accountChangedSubscription: Subscription;

  constructor(private scoringApiClient: ScoringApiClient,
              private authenticationService: AuthenticationService) {
    this.accountChangedSubscription = this.authenticationService.accountChanged.subscribe(() => this.loadData());
  }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.reloadProjectsForScoringAsync();
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
    const response = await this.scoringApiClient.getProjectForScoringAsync(<ProjectsForScoringRequest>{
      expertiseArea: <number>expertiseArea
    });
    this.projects = response.items.map(p => Project.createProject(p));
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
