import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {BalanceService} from './balance.service';

@Injectable()
export class ShouldHaveEthGuard implements CanActivate {

  constructor(private router: Router, private balanceService: BalanceService) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {

    return await this.balanceService.checkEthAsync();
  }
}
