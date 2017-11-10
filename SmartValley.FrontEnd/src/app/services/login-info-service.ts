import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';

@Injectable()
export class LoginInfoService {
  public saveLoginInfo(account: string, signature: string) {
    window.localStorage[account] = signature;
  }

  public isLoggedIn(from: string): boolean {
    return !isNullOrUndefined(window.localStorage[from]);
  }
}
