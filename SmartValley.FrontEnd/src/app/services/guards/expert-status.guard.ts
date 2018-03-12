import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {UserContext} from '../authentication/user-context';

@Injectable()
export class ExpertStatusGuard implements CanActivate {
  constructor(private router: Router,
              private expertApiClient: ExpertApiClient,
              private userContext: UserContext) {
  }

  public async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const address = this.userContext.getCurrentUser().account;
    const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
    return route.data.expertStatuses.includes(expertStatusResponse.status);
  }
}
