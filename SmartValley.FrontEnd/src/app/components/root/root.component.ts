import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication-service';
import {Web3Service} from '../../services/web3-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {NotificationsService} from 'angular2-notifications';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.css']
})
export class RootComponent implements OnInit {

  public userInfo: User;

  constructor(private web3Service: Web3Service,
              private authenticationService: AuthenticationService,
              private notificationsService: NotificationsService,
              private router: Router) {
    // this.authenticationService.accountChanged.subscribe(async () => await this.updateUserInfo());
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
    const isOk = await this.authenticationService.authenticateAsync();
    if (isOk) {
      await this.router.navigate([Paths.Application]);
    }
  }
}
