import {GuardType} from './guard-type.enum';
import {ShouldHaveEthGuard} from '../balance/should-have-eth.guard';
import {ShouldHaveSvtGuard} from '../balance/should-have-svt.guard';
import {Injectable} from '@angular/core';
import {BalanceService} from '../balance/balance.service';
import {CanActivate} from '@angular/router';

@Injectable()
export class GuardFactory {
  constructor(private balanceService: BalanceService) {
  }

  public create(type: GuardType): CanActivate {
    switch (type) {
      case GuardType.ShouldHaveEth:
        return new ShouldHaveEthGuard(this.balanceService);
      case GuardType.ShouldHaveSvt:
        return new ShouldHaveSvtGuard(this.balanceService);
      default:
        throw Error('Unknown guard type.');
    }
  }
}
