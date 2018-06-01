import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot} from '@angular/router';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class ShouldNotBeExpertGuard implements CanActivate {
  constructor(private userContext: UserContext) {
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return !this.userContext.getCurrentUser().isExpert;
  }
}
