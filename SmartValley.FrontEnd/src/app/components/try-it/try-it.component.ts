import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Web3Service} from '../../services/web3-service';
import {AuthenticationService} from '../../services/authentication-service';
import {Paths} from '../../paths';
import {NotificationService} from '../../services/notification-service';

@Component({
  selector: 'app-try-it',
  templateUrl: './try-it.component.html',
  styleUrls: ['./try-it.component.css']
})

export class TryItComponent {

  errorMessage: string;

  constructor(private router: Router,
              private web3Service: Web3Service,
              private authenticationService: AuthenticationService,
              private notificationService: NotificationService) {
  }

  async tryIt() {
    try {
      const isRinkeby = await
        this.web3Service.isRinkebyNetwork();
      if (!isRinkeby) {
        this.notificationService.notify('warn', 'Please switch to the Rinkeby network');
        return;
      }
    } catch (reason) {
      this.notificationService.notify('error', reason);
      return;
    }
    const isOk = await this.authenticationService.authenticate();
    if (isOk) {
      await this.router.navigate([Paths.Root]);
    }
  }
}
