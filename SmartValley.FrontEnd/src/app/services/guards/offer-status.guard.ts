import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {OffersApiClient} from '../../api/scoring-offer/offers-api-client';

@Injectable()
export class OfferStatusGuard implements CanActivate {

  constructor(private offersApiClient: OffersApiClient) {
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const projectId = +route.paramMap.get('id');
    const areaType = +route.paramMap.get('areaType');

    const statusResponse = await this.offersApiClient.getStatusAsync(projectId, areaType);
    return route.data.offerStatuses.includes(statusResponse.status);
  }
}
