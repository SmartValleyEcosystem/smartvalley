import { Component } from '@angular/core';
import { isNullOrUndefined } from 'util';
import { Router } from '@angular/router';
import { Web3Service } from '../../services/web3-service';

@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.css']
})

export class LandingComponent {

  errorMessage: string;

  constructor(private router: Router, private web3Service: Web3Service) {
  }

  async tryIt() {
    this.web3Service.initialize();

    if (this.web3Service.isAvailable()) {
      try {
        const isRinkeby = await this.web3Service.isRinkeby();
        if (!isRinkeby) {
          this.showError('Please switch to the Rinkeby network');
          return;
        }
      } catch (reason) {
        this.showError(reason);
        return;
      }

      const from = this.web3Service.getAccount();
      if (this.isLoggedIn(from)) {
        await this.navigateTo('/loggedin');
        return;
      }

      try {
        window.localStorage[from] = await this.web3Service.sign('Confirm login', from);
        await this.navigateTo('/loggedin');
      } catch (reason) {
        this.showError(reason);
      }
    } else {
      await this.navigateTo('/metamaskhowto');
    }
  }

  private showError(message: string) {
    this.errorMessage = message;
  }

  private isLoggedIn(from: string): boolean {
    return !isNullOrUndefined(window.localStorage[from]);
  }

  private async navigateTo(path: string): Promise<any> {
      await this.router.navigate([path]);
  }
}
