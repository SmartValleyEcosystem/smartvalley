import {AfterViewChecked, Component, ViewChild} from '@angular/core';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ActivatedRoute, Router} from '@angular/router';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {QuestionService} from '../../services/question-service';
import {Question} from '../../services/question';
import {Estimate} from '../../services/estimate';
import {EstimatesApiClient} from '../../api/estimates/estimates-api-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {ProjectService} from '../../services/project-service';
import {BlockiesService} from '../../services/blockies-service';
import {NgbTabset} from '@ng-bootstrap/ng-bootstrap';
import {TeamMember} from '../../services/team-member';
import {Paths} from '../../paths';
import {Constants} from '../../constants';
import {QuestionWithEstimatesResponse} from '../../api/estimates/question-with-estimates-response';

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
  private knownTabs = [Constants.ReportFormTab, Constants.ReportEstimatesTab];

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
      this.selectedTab = params[Constants.TabQueryParam];
    });
  }

  ngAfterViewChecked(): void {
    if (this.tabSet && this.knownTabs.includes(this.selectedTab)) {
      this.tabSet.select(this.selectedTab);
    }
  }


  onExpertiseTabChanged($event: any) {
    let expertiseArea: ExpertiseArea = 1;
    const index: number = $event.index;
    switch (index) {
      case 0 :
        expertiseArea = ExpertiseArea.HR;
        break;
      case 1 :
        expertiseArea = ExpertiseArea.Lawyer;
        break;
      case 2 :
        expertiseArea = ExpertiseArea.Analyst;
        break;
      case 3 :
        expertiseArea = ExpertiseArea.TechnicalExpert;
        break;
    }
    this.loadExpertEstimates(expertiseArea);
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
    this.loadExpertEstimates(ExpertiseArea.HR);
  }

  private getMembersCollection(report: ProjectDetailsResponse): Array<TeamMember> {
    const result: TeamMember[] = [];
    const memberTypeNames = Object.keys(EnumTeamMemberType).filter(key => !isNaN(Number(EnumTeamMemberType[key])));

    for (const memberType of memberTypeNames) {
      const teamMember = report.teamMembers.find(value => value.memberType === EnumTeamMemberType[memberType])
        || <TeamMember>{memberType: EnumTeamMemberType[memberType], fullName: '\u2014'};

      result.push(<TeamMember>{
        memberType: teamMember.memberType,
        facebookLink: teamMember.facebookLink,
        linkedInLink: teamMember.linkedInLink,
        fullName: teamMember.fullName
      });
    }
    return result;
  }

  private async loadExpertEstimates(expertiseArea: ExpertiseArea) {
    const estimatesResponse = await this.estimatesApiClient.getByProjectIdAndExpertiseAreaAsync(this.projectId, expertiseArea);
    const fullQuestions = await this.questionService.getByCategory(expertiseArea);
    this.categoryAverageScore = estimatesResponse.averageScore;
    this.questions = estimatesResponse.questions.map(i =>
      <Question>{
        name: fullQuestions.filter(j => j.id === i.questionId)[0].name,
        description: fullQuestions.filter(j => j.id === i.questionId)[0].description,
        estimates: i.estimates.map(j => <Estimate>{score: j.score, comments: j.comment})
      });
  }

  showEstimates() {
    this.tabSet.select('estimates');
  }

  showForm() {
    this.tabSet.select('form');
  }
}
