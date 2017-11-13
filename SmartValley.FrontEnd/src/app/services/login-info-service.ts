import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';


@Injectable()
export class LoginInfoService {

  static MESSAGE_TO_SIGN = 'signme';
  static ETH_ADDRESS_KEY = 'eth-address';
  static SIGNATURE_KEY = 'signature';

  public saveLoginInfo(account: string, signature: string) {
    window.localStorage[LoginInfoService.ETH_ADDRESS_KEY] = account;
    window.localStorage[LoginInfoService.SIGNATURE_KEY] = signature;
  }

  public isLoggedInBy(from: string): boolean {
    const address = window.localStorage[LoginInfoService.ETH_ADDRESS_KEY];
    return from === address;
  }
  public isAuthenticated(): boolean {
    const address = window.localStorage[LoginInfoService.ETH_ADDRESS_KEY];
    return !isNullOrUndefined(address);
  }
}
