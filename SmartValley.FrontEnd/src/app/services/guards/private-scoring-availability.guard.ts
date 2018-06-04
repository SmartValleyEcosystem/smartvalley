import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import {UserContext} from '../authentication/user-context';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {OffersApiClient} from '../../api/scoring-offer/offers-api-client';
import {OffersOrderBy} from '../../api/scoring-offer/offers-order-by';
import {OffersQuery} from '../../api/scoring-offer/offers-query';
import {OfferStatus} from '../../api/scoring-offer/offer-status.enum';
import {SortDirection} from '../../api/sort-direction.enum';

@Injectable()
export class PrivateScoringAvailabilityGuard implements CanActivate {

  constructor(private projectApiClient: ProjectApiClient,
              private offersApiClient: OffersApiClient,
              private userContext: UserContext) {
  }

  async canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Promise<boolean> {
      const user = this.userContext.getCurrentUser();
      const projectId = +next.paramMap.get('id');
      const currentProject = await this.projectApiClient.getProjectSummaryAsync(projectId);

      const isAdmin = user.isAdmin;
      const isExpert = user.isExpert;
      const isProjectOwner = currentProject.authorId === user.id;
      let isOfferOwner = false;
      const expertOffers = await this.offersApiClient.queryAsync(<OffersQuery> {
          offset: 0,
          count: 1,
          expertId: user.id,
          projectId: projectId
      });
      isOfferOwner = (expertOffers.items.length > 0);

      return isAdmin || isProjectOwner || !currentProject.isPrivate || (isExpert && isOfferOwner);
  }
}
