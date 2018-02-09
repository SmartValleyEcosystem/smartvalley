import {Injectable} from '@angular/core';
import {Paths} from '../../paths';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class ExpertGuard implements CanActivate {
  constructor(private router: Router,
              private expertApiClient: ExpertApiClient,
              private userContext: UserContext) {
  }

  public async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const address = this.userContext.getCurrentUser().account;
    const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
    if (!expertStatusResponse.isApplied) {
      this.router.navigate([Paths.BecomeExpert]);
      return false;
    } else if (!expertStatusResponse.isConfirmed) {
      this.router.navigate([Paths.RegisterExpert]);
      return false;
    } else {
      return true;
    }
  }
}
