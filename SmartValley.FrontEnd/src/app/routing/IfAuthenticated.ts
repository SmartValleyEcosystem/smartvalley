import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {AuthenticationService} from '../services/authentication-service';
import {Paths} from '../paths';

@Injectable()
export class IfAuthenticated implements CanActivate {
  constructor(private router: Router,
              private authenticationService: AuthenticationService) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {

    const userInfo = await this.authenticationService.getUserInfo();
    if(userInfo == null) {
      this.router.navigate([Paths.Root]);
    }
    return userInfo != null && userInfo.isAuthenticated;

  }
}

