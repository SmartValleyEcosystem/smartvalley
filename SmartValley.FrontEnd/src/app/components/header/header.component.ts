import {Component, OnInit} from '@angular/core';
import {AuthenticationService} from '../../services/authentication/authentication-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {BalanceService} from '../../services/balance/balance.service';
import {Balance} from '../../services/balance/balance';
import {BlockiesService} from '../../services/blockies-service';
import {UserContext} from '../../services/authentication/user-context';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {isNullOrUndefined} from 'util';
import {ProjectService} from '../../services/project/project.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  public currentBalance: number;
  public showReceiveEtherButton: boolean;
  public isAuthenticated: boolean;
  public isAdmin: boolean;
  public haveProject: boolean;
  public accountAddress: string;
  public accountImgUrl: string;
  public projectsLink: string;
  public accountLink: string;
  public adminPanelLink: string;
  public myProjectLink: string;

  constructor(private router: Router,
              private balanceService: BalanceService,
              private blockiesService: BlockiesService,
              private authenticationService: AuthenticationService,
              private projectApiClient: ProjectApiClient,
              private userContext: UserContext,
              private expertApiClient: ExpertApiClient,
              private projectService: ProjectService) {
    this.projectService.projectsDeleted.subscribe(async () => {
      this.haveProject = false;
      this.myProjectLink = '';
    });
    this.projectService.projectsCreated.subscribe(async () => await this.updateProjectsAsync();
    this.balanceService.balanceChanged.subscribe((balance: Balance) => this.updateBalance(balance));
    this.userContext.userContextChanged.subscribe(async (user) => await this.updateAccountAsync(user));
  }

  async ngOnInit() {
    const currentUser = this.userContext.getCurrentUser();
    await this.updateAccountAsync(currentUser);
    this.projectsLink = Paths.ProjectList;
    this.accountLink = Paths.Account;
    this.adminPanelLink = Paths.Admin;
    await this.balanceService.updateBalanceAsync();
  }

  private async updateProjectsAsync(): Promise<void> {
    const myProjectIdResponse = await this.projectApiClient.getMyProjectAsync();
    if (!isNullOrUndefined(myProjectIdResponse)) {
      this.myProjectLink = Paths.MyProject + '/' + myProjectIdResponse.id;
      this.haveProject = true;
    }
  }

  private async updateAccountAsync(user: User): Promise<void> {
    if (user) {
      this.updateProjectsAsync();
      this.isAuthenticated = true;
      this.accountAddress = user.account;
      this.accountImgUrl = this.blockiesService.getImageForAddress(user.account);
      this.isAdmin = user.roles.includes('Admin');
    } else {
      this.isAuthenticated = false;
      this.isAdmin = false;
      this.haveProject = false;
      this.myProjectLink = '';
    }
  }

  public async loginAsync(): Promise<void> {
    await this.authenticationService.authenticateAsync();
  }

  public logout(): void {
    this.userContext.deleteCurrentUser();
  }

  private updateBalance(balance: Balance): void {
    if (balance != null) {
      this.currentBalance = balance.ethBalance;
      this.showReceiveEtherButton = !balance.wasEtherReceived;
    } else {
      this.showReceiveEtherButton = false;
    }
  }

  async navigateToExpertApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      const address = this.userContext.getCurrentUser().account;
      const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
      if (expertStatusResponse.status === ExpertApplicationStatus.Pending) {
        await this.router.navigate([Paths.ExpertStatus]);
      } else if (expertStatusResponse.status === ExpertApplicationStatus.Accepted) {
        await this.router.navigate([Paths.Expert]);
      } else {
        await this.router.navigate([Paths.RegisterExpert]);
      }
    }
  }

  async navigateToProjectApplication() {
    if (await this.authenticationService.authenticateAsync()) {
      await this.router.navigate([Paths.Project]);
    }
  }
}
