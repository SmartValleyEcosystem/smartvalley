import {AfterViewInit, Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Project} from '../../services/project';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {ActivatedRoute} from '@angular/router';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ProjectsForScoringRequest} from '../../api/scoring/projecs-for-scoring-request';
import {Constants} from '../../constants';
import {isNullOrUndefined} from 'util';
import {Subscription} from 'rxjs/Subscription';
import {ProjectResponse} from '../../api/project/project-response';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent implements OnDestroy, OnInit {
  public projectsForScoring: Array<Project> = [];
  public myProjects: Array<Project> = [];
  public selectedExpertiseTabIndex: number;

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
    this.selectedExpertiseTabIndex = index;
    await this.reloadProjectsForScoringAsync();
  }

  public ngOnDestroy(): void {
    if (!isNullOrUndefined(this.accountChangedSubscription) && !this.accountChangedSubscription.closed) {
      this.accountChangedSubscription.unsubscribe();
    }
  }

  private async reloadProjectsForScoringAsync(): Promise<void> {
    const expertiseArea = this.getExpertiseAreaByIndex(this.selectedExpertiseTabIndex);
    const response = await this.scoringApiClient.getProjectForScoringAsync(<ProjectsForScoringRequest>{
      expertiseArea: <number>expertiseArea
    });
    this.projectsForScoring = response.items.map(p => this.createProject(p, expertiseArea));
  }

  private createProject(response: ProjectResponse, expertiseArea: ExpertiseArea = ExpertiseArea.HR): Project {
    return <Project>{
      id: response.id,
      name: response.name,
      area: response.area,
      country: response.country,
      score: response.score,
      description: response.description,
      address: response.address,
      expertiseArea: expertiseArea
    };
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
