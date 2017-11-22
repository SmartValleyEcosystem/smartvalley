import {Component} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from '../../services/web3-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Project} from '../../services/project';
import {NotificationsService} from 'angular2-notifications';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent {

  public projects: Array<Project>;

  constructor(private authenticationService: AuthenticationService,
              private router: Router) {

    this.initTestData();
  }

  async navigateToScoring() {
    await this.router.navigate([Paths.Scoring]);
  }

  async createProject() {
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Application]);
    }
  }

  colorOfProjectRate(rate: string) {
    const r = parseInt(rate, 10)
    if (r > 80) { return 'high_rate'; }
    if (r > 45) { return 'medium_rate'; }
    return 'low_rate';
  }

  // тестовые данные
  initTestData() {
    this.projects = [];
    this.projects.push(<Project>{
      projectName: 'Rega Risk Sharing',
      projectArea: 'Crowdsurance',
      projectCountry: 'Russia',
      scoringRating: '99',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.projects.push(<Project>{
      projectName: 'BitClave Active Search Ecosystem',
      projectArea: 'Rotetechnology',
      projectCountry: 'Russia',
      scoringRating: '65',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.projects.push(<Project>{
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: '8',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });
  }
}
