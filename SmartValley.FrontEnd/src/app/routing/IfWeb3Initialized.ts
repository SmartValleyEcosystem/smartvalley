import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Web3Service} from '../services/web3-service';
import {Injectable} from '@angular/core';
import {Paths} from '../paths';

@Injectable()
export class IfWeb3Initialized implements CanActivate {
  constructor(private web3Service: Web3Service,
              private router: Router) {
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
