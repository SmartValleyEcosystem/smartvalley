import {ScoringOfferStatusResponse} from './scoring-offer-status-response';
import {HttpClient, HttpParams} from '@angular/common/http';
import {BaseApiClient} from '../base-api-client';
import {ChangeStatusExpertOfferRequest} from '../expert/change-status-expert-offer-request';
import {UpdateOffersRequest} from '../expert/update-offers-request';
import {ScoringOfferResponse} from './scoring-offer-response';
import {CollectionResponse} from '../collection-response';
import {isNullOrUndefined} from 'util';
import {OffersQuery} from './offers-query';
import {Injectable} from '@angular/core';
import {AreaType} from '../scoring/area-type.enum';

@Injectable()
export class OffersApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getStatusAsync(projectId: number, areaType: number): Promise<ScoringOfferStatusResponse> {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('areaType', areaType.toString());

    return this.http.get<ScoringOfferStatusResponse>(`${this.baseApiUrl}/scoring/offers/status`, {
      params: parameters
    }).toPromise();
  }

  public async acceptAsync(transactionHash: string, scoringId: number, areaId: AreaType) {
    await this.http.put(this.baseApiUrl + '/scoring/offers/accept/', <ChangeStatusExpertOfferRequest> {
      transactionHash: transactionHash,
      scoringId: scoringId,
      areaId: areaId
    }).toPromise();
  }

  public async declineAsync(transactionHash: string, scoringId: number, areaId: AreaType) {
    await this.http.put(this.baseApiUrl + '/scoring/offers/reject/', <ChangeStatusExpertOfferRequest>{
      transactionHash: transactionHash,
      scoringId: scoringId,
      areaId: areaId
    }).toPromise();
  }

  public async updateAsync(projectExternalId: string, transactionHash: string): Promise<void> {
    await this.http.put(this.baseApiUrl + '/scoring/offers', <UpdateOffersRequest>{
      projectExternalId: projectExternalId,
      transactionHash: transactionHash
    }).toPromise();
  }

  public async queryAsync(query: OffersQuery): Promise<CollectionResponse<ScoringOfferResponse>> {
    const checkParam = (param) => isNullOrUndefined(param) ? '' : param.toString();

    let parameters = new HttpParams()
      .append('offset', query.offset.toString())
      .append('count', query.count.toString())
      .append('orderBy', checkParam(query.orderBy))
      .append('sortDirection', checkParam(query.sortDirection))
      .append('expertId', checkParam(query.expertId))
      .append('projectId', checkParam(query.projectId))
      .append('scoringId', checkParam(query.scoringId));

    if (query.statuses) {
      query.statuses.forEach(id => {
        parameters = parameters.append('statuses', id.toString());
      });
    }

    return this.http.get<CollectionResponse<ScoringOfferResponse>>(
      `${this.baseApiUrl}/scoring/offers`,
      {params: parameters})
      .toPromise();
  }
}
