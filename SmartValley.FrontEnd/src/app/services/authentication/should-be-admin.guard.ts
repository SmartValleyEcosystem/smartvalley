import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {AuthenticationService} from './authentication-service';
import {Paths} from '../../paths';
import {UserContext} from './user-context';

@Injectable()
export class ShouldBeAdminGuard implements CanActivate {
  constructor(private router: Router,
              private authenticationService: AuthenticationService,
              private userContext: UserContext) {
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const user = this.userContext.getCurrentUser();
    if (this.authenticationService.isAuthenticated() && user.roles.includes('Admin')) {
      return true;
    } else {
      this.router.navigate([Paths.Root]);
      return false;
    }
  }
}
