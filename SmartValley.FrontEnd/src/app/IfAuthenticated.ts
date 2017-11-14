import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {Web3Service} from './services/web3-service';
import {AuthenticationService} from './services/authentication-service';
import {Paths} from './paths';


@Injectable()
export class IfWeb3Initialized implements CanActivate {
  constructor(protected web3Service: Web3Service,
              protected router: Router) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {

    if (!this.web3Service.isInitialized) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return false;
    }
    return true;

  }
}

@Injectable()
export class IfAuthenticated implements CanActivate {
  constructor(protected web3Service: Web3Service,
              protected router: Router,
              protected authenticationService: AuthenticationService) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {

    const userInfo = await this.authenticationService.getUserInfo();
    console.log(userInfo);
    if(userInfo == null){
      this.router.navigate([Paths.TryIt]);
    }
    return userInfo != null && userInfo.isAuthenticated;

  }
}

