import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {Web3Service} from './services/web3-service';
import {LoginInfoService} from './services/login-info-service';
import {Paths} from './paths';

@Injectable()
export class IsAuthorizedGuard implements CanActivate {
  constructor(
    private web3Service: Web3Service,
    private router: Router,
    private loginService: LoginInfoService) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (!this.web3Service.isAvailable()) {
      this.router.navigate([Paths.MetaMaskHowTo]);
      return false;
    }

    const account = this.web3Service.getAccount();
    if (this.loginService.isLoggedInBy(account)) {
      return true;
    }

    this.router.navigate([Paths.Landing]);
    return false;
  }
}
