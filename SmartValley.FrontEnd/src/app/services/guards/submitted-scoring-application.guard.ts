import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {Paths} from '../../paths';

@Injectable()
export class SubmittedScoringApplicationGuard implements CanActivate {

  constructor(private scoringApplicationApiClient: ScoringApplicationApiClient) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {
    const projectId = +next.paramMap.get('id');
    const application = await this.scoringApplicationApiClient.getScoringApplicationsAsync(projectId);
    return application.isSubmitted === next.data.shouldBeSubmitted;
  }
}
