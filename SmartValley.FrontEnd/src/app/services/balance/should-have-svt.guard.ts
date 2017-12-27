import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {BalanceService} from './balance.service';

@Injectable()
export class ShouldHaveSvtGuard implements CanActivate {
  constructor(private balanceService: BalanceService) {
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return this.balanceService.checkSvtForProjectAsync();
  }
}
