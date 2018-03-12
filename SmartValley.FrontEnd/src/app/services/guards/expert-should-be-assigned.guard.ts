import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {OffersApiClient} from '../../api/expert/offers-api-client';
import {OfferStatus} from '../../api/expert/offer-status.enum';

@Injectable()
export class ExpertShouldBeAssignedGuard implements CanActivate {

  constructor(private router: Router,
              private offersApiClient: OffersApiClient) {
  }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    const projectId = +route.paramMap.get('id');
    const areaType = +route.queryParamMap.get('areaType');

    const statusResponse = await this.offersApiClient.getOfferStatusAsync(projectId, areaType);
    return statusResponse.status === OfferStatus.Accepted;
  }
}
