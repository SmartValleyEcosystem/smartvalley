import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Web3Service} from '../../services/web3-service';
import {LoginInfoService} from '../../services/login-info-service';
import {Paths} from '../../paths';
import {isNullOrUndefined} from 'util';
import {NotificationService} from '../../services/notification-service';


@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})

export class LandingComponent {

  constructor(private router: Router,
              private web3Service: Web3Service,
              private loginService: LoginInfoService,
              private motificationService: NotificationService) {
    this.init();
  }

  async init() {
    if (this.web3Service.isAvailable()) {
      try {
        const isRinkeby = await this.web3Service.isRinkebyNetwork();
        if (!isRinkeby) {
          this.motificationService.notify('error', 'Please switch to the Rinkeby network');
          return;
        }
      } catch (reason) {
        this.motificationService.notify('error', reason);
        return;
      }

      const from = this.web3Service.getAccount();
      if (isNullOrUndefined(from)) {
        this.motificationService.notify('error', 'Please log in to MetaMask and refresh.');
        return;
      }
      if (this.loginService.isLoggedInBy(from)) {
        await this.router.navigate([Paths.LoggedIn]);
        return;
      }

      try {
        const signature = await this.web3Service.sign( LoginInfoService.MESSAGE_TO_SIGN, from);
        this.loginService.saveLoginInfo(from, signature);

        await this.router.navigate([Paths.LoggedIn]);
      } catch (reason) {
        this.motificationService.notify('error', reason);
      }
    } else {
      await this.router.navigate([Paths.MetaMaskHowTo]);
    }
  }
}
