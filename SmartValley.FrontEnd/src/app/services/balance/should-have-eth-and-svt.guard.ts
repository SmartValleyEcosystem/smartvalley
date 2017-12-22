import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {BalanceService} from './balance.service';

@Injectable()
export class ShouldHaveEthAndSvtGuard implements CanActivate {

  constructor(private router: Router, private balanceService: BalanceService) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {

    const isEthReceived = await this.balanceService.checkEthAsync();
    const isSvtReceived = await this.balanceService.checkSvtForProjectAsync();

    return isSvtReceived && isEthReceived;
  }
}
