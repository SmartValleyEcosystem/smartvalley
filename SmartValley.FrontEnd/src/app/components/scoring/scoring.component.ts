import {Component} from '@angular/core';
import {Scoring} from '../../services/scoring';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';
import {ScoringCategory} from '../../api/scoring/scoring-category.enum';
import {Paths} from '../../paths';


@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public scorings: Array<Scoring>;

  constructor(private scoringApiClient: ScoringApiClient) {
    this.getProjectsForCategory(ScoringCategory.Hr);
  }

  tabChanged($event: any) {
    let scroringCategory: ScoringCategory = 1;
    let index: number = $event.index;
    switch (index) {
      case 0 :
        scroringCategory = ScoringCategory.Hr;
        break;
      case 1 :
        scroringCategory = ScoringCategory.Lawyer;
        break;
      case 2 :
        scroringCategory = ScoringCategory.Analyst;
        break;
      case 3 :
        scroringCategory = ScoringCategory.Tech;
        break;
    }
    this.getProjectsForCategory(scroringCategory);
  }


  private async getProjectsForCategory(scroringCategory: ScoringCategory) {
    this.scorings = [];
    const projects = await this.scoringApiClient.getProjectForScoringAsync({scroringCategory: scroringCategory});
    for (const response of projects) {
      this.scorings.push(<Scoring>{
        projectId: response.id,
        projectName: response.projectName,
        projectArea: response.projectArea,
        projectCountry: response.projectCountry,
        scoringRating: response.scoringRating,
        projectDescription: response.projectDescription,
        projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
      });
    }
  }

  showProject(id: number) {
    this.router.navigate([Paths.Scoring + '/' + id]);
  }
}
