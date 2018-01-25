import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {BalanceService} from './balance.service';
import {SvtRequiredType} from './svt-required-type.enum';

@Injectable()
export class ShouldHaveSvtGuard implements CanActivate {
  constructor(private balanceService: BalanceService) {
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const requiredFor = next.data['requiredFor'] as SvtRequiredType;
    switch (requiredFor) {
      case SvtRequiredType.ForScoring:
        return this.balanceService.checkSvtForScoringAsync();
      case SvtRequiredType.GreaterThanZero:
        return this.balanceService.checkSvtGreaterThanZero();
    }

  }
}
