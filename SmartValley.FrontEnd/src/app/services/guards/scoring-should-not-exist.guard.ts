import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {ScoringApplicationApiClient} from '../../api/scoring-application/scoring-application-api-client';
import {ScoringApiClient} from '../../api/scoring/scoring-api-client';

@Injectable()
export class ScoringShouldNotExistGuard implements CanActivate {
  constructor(private scoringApiClient: ScoringApiClient) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {
    const projectId = +next.paramMap.get('id');
    const scoring = await this.scoringApiClient.getByProjectIdAsync(projectId);
    return scoring.id === null;
  }
}
