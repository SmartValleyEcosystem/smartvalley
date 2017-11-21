import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from '../../services/web3-service';
import {NotificationService} from '../../services/notification-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Project} from '../../services/project';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent implements OnInit {

  public userInfo: User;
  public projects: Array<Project>;

  constructor(private web3Service: Web3Service,
              private authenticationService: AuthenticationService,
              private notificationService: NotificationService,
              private router: Router) {
    // this.authenticationService.userInfoChanged.subscribe(async () => await this.updateUserInfo());
    this.initTestData();
  }

  async ngOnInit() {
  //  await this.updateUserInfo();
  }

  async updateUserInfo() {
  //  this.userInfo = await this.authenticationService.getCurrentUser();
  }

  async navigateToScoring() {
    await this.router.navigate([Paths.Scoring]);
  }

  async createProject() {
    const isOk = await this.authenticationService.authenticate();
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
