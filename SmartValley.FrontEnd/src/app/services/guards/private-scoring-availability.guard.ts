import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {UserContext} from '../authentication/user-context';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {OffersApiClient} from '../../api/scoring-offer/offers-api-client';
import {OffersQuery} from '../../api/scoring-offer/offers-query';

@Injectable()
export class PrivateScoringAvailabilityGuard implements CanActivate {

  constructor(private projectApiClient: ProjectApiClient,
              private offersApiClient: OffersApiClient,
              private userContext: UserContext) {
  }

  async canActivate(next: ActivatedRouteSnapshot,
                    state: RouterStateSnapshot): Promise<boolean> {
    const user = this.userContext.getCurrentUser();
    const projectId = +next.paramMap.get('id');
    const currentProject = await this.projectApiClient.getProjectSummaryAsync(projectId);

    if (!currentProject.isPrivate) {
      return true;
    }

    const isAdmin = user.isAdmin;
    const isExpert = user.isExpert;
    const isProjectOwner = currentProject.authorId === user.id;
    const expertOffers = await this.offersApiClient.queryAsync(<OffersQuery> {
      offset: 0,
      count: 1,
      expertId: user.id,
      projectId: projectId
    });
    const isOfferOwner = (expertOffers.items.length > 0);

    return isAdmin || isProjectOwner || !currentProject.isPrivate || (isExpert && isOfferOwner);
  }
}
