import {AfterViewChecked, AfterViewInit, Component, ViewChild} from '@angular/core';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {Estimate} from '../../services/estimate';
import {isNullOrUndefined} from 'util';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {TeamMember} from '../../services/team-member';
import {Paths} from '../../paths';
import {Constants} from '../../constants';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements AfterViewChecked {
  public questions: Array<Question>;
  public report: ProjectDetailsResponse;
  public EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;
  public categoryAverageScore: number;
  public teamMembers: Array<TeamMember>;
  public projectImageUrl: string;

  private projectId: number;

  @ViewChild('reportTabSet')
  private tabSet: NgbTabset;
  private selectedTab: string;

  constructor(private projectApiClient: ProjectApiClient,
              private estimatesApiClient: EstimatesApiClient,
              private questionService: QuestionService,
              private route: ActivatedRoute,
              private blockiesService: BlockiesService,
              public projectService: ProjectService,
              private activatedRoute: ActivatedRoute,
              private router: Router) {
    this.loadInitialData();
    this.activatedRoute.queryParams.subscribe(params => {
      const value = params[Constants.TabQueryParam] || 'none';
      this.selectedTab = value;
    });
  }

  ngAfterViewChecked(): void {
    if (this.tabSet) {
      switch (this.selectedTab) {
        case Constants.ReportFormTab:
          this.showForm();
          break;
        case Constants.ReportEstimatesTab:
          this.showEstimates();
          break;
      }
    }
  }


  onExpertiseTabChanged($event: any) {
    let scoringCategory: ScoringCategory = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        scoringCategory = ScoringCategory.HR;
        break;
      case 1 :
        scoringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scoringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scoringCategory = ScoringCategory.TechnicalExpert;
        break;
    }
    this.loadExpertEstimates(scoringCategory);
  }

  onMainTabChanged($event: any) {
    const queryParams = Object.assign({}, this.activatedRoute.snapshot.queryParams);
    queryParams[Constants.TabQueryParam] = $event.nextId;
    this.router.navigate([Paths.Report + '/' + this.projectId], {queryParams: queryParams, replaceUrl: true});
  }

  private async loadInitialData() {
    this.projectId = +this.route.snapshot.paramMap.get('id');
    this.report = await this.projectApiClient.getDetailsByIdAsync(this.projectId);
    this.teamMembers = this.getMembersCollection(this.report);
    this.projectImageUrl = this.blockiesService.getImageForAddress(this.report.projectAddress);
    this.loadExpertEstimates(ScoringCategory.HR);
  }

  private getMembersCollection(report: ProjectDetailsResponse): Array<TeamMember> {
    const result: TeamMember[] = [];
    const memberTypeNames = Object.keys(EnumTeamMemberType).filter(key => !isNaN(Number(EnumTeamMemberType[key])));

    for (const memberType of memberTypeNames) {
      const teamMember = report.teamMembers.find(value => value.memberType === EnumTeamMemberType[memberType])
        || <TeamMember>{memberType: EnumTeamMemberType[memberType], fullName: '-'};

      result.push(<TeamMember>{
        memberType: teamMember.memberType,
        facebookLink: teamMember.facebookLink,
        linkedInLink: teamMember.linkedInLink,
        fullName: teamMember.fullName
      });
    }
    return result;
  }

  private async loadExpertEstimates(scoringCategory: ScoringCategory) {
    const estimatesResponse = await this.estimatesApiClient.getByProjectIdAndCategoryAsync(this.projectId, scoringCategory);
    this.categoryAverageScore = estimatesResponse.averageScore;
    const questions = this.questionService.getByExpertType(scoringCategory);

    for (const question of questions) {
      question.estimates = [];
      const estimates = estimatesResponse.items.filter(e => e.questionIndex === question.indexInCategory);
      for (const estimate of estimates) {
        if (isNullOrUndefined(estimate)) {
          continue;
        }
        question.estimates.push(<Estimate>{
          score: estimate.score,
          comments: estimate.comment
        });
      }
    }
    this.questions = questions;
  }

  showEstimates() {
    this.tabSet.select('estimates');
  }

  showForm() {
    this.tabSet.select('form');
  }
}
