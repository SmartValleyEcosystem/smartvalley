import {EventEmitter, Injectable} from '@angular/core';
import {User} from './user';

@Injectable()
export class UserContext {

  private userKey = 'userKey';

  constructor() {
  }

  public userContextChanged: EventEmitter<any> = new EventEmitter<any>();

  public getCurrentUser(): User {
    const userOptions = JSON.parse(localStorage.getItem(this.userKey));
    let user = null;
    if (userOptions) {
      user = new User(
        userOptions.id,
        userOptions.account,
        userOptions.signature,
        userOptions.token,
        userOptions.email,
        userOptions.roles
      );
    }
    return user;
  }

  public saveCurrentUser(user: User) {
    if (this.isCurrentUserEquals(user)) {
      return;
    }
    localStorage.setItem(this.userKey, JSON.stringify(user));
    this.userContextChanged.emit(user);
  }

  public deleteCurrentUser() {
    localStorage.removeItem(this.userKey);
    this.userContextChanged.emit(null);
  }

  public getSignatureByAccount(account: string): string {
    return localStorage.getItem(account);
  }

  public saveSignatureForAccount(account: string, signature: string) {
    localStorage.setItem(account, signature);
  }

  private isCurrentUserEquals(user: User): boolean {
    const currentUser = this.getCurrentUser();
    if (currentUser != null && user.account === currentUser.account
      && user.signature === currentUser.signature
      && user.token === currentUser.token
      && user.roles.length === currentUser.roles.length
      && user.roles.every((v, i) => v === currentUser.roles[i])) {
      return true;
    }
    return false;
  }

}
