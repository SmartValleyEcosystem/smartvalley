import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {AuthenticationService} from './authentication-service';
import {Paths} from '../../paths';

@Injectable()
export class ShouldBeAdminGuard implements CanActivate {
  constructor(private router: Router,
              private authenticationService: AuthenticationService) {
  }

  public canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const user = this.authenticationService.getCurrentUser();
    if (this.authenticationService.isAuthenticated() && user.isAdmin) {
      return true;
    } else {
      this.router.navigate([Paths.Root]);
      return false;
    }
  }
}
