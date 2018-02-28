import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {UserContext} from '../authentication/user-context';
import {ExpertApplicationStatus} from '../expert/expert-application-status.enum';

@Injectable()
export class ExpertStatusGuard implements CanActivate {
  constructor(private router: Router,
              private expertApiClient: ExpertApiClient,
              private userContext: UserContext) {
  }

  public async canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const address = this.userContext.getCurrentUser().account;
    const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
    if (expertStatusResponse.status !== ExpertApplicationStatus.Accepted) {
      return true;
    } else {
      return false;
    }
  }
}
