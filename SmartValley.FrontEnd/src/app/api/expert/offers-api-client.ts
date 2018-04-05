import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {ScoringOfferResponse} from './scoring-offer-response';
import {AreaType} from '../scoring/area-type.enum';
import {ScoringOfferStatusResponse} from './scoring-offer-status-response';
import {UpdateOffersRequest} from './update-offers-request';
import {ChangeStatusExpertOfferRequest} from './change-status-expert-offer-request';
import {isNullOrUndefined} from 'util';
import {OffersQuery} from './offers-query';

@Injectable()
export class OffersApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getOfferStatusAsync(projectId: number, areaType: number): Promise<ScoringOfferStatusResponse> {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('areaType', areaType.toString());

    return this.http.get<ScoringOfferStatusResponse>(`${this.baseApiUrl}/scoring/offers/status`, {
      params: parameters
    }).toPromise();
  }

  public async acceptExpertOfferAsync(transactionHash: string, scoringId: number, areaId: AreaType) {
    await this.http.put(this.baseApiUrl + '/scoring/offers/accept/', <ChangeStatusExpertOfferRequest> {
      transactionHash: transactionHash,
      scoringId: scoringId,
      areaId: areaId
    }).toPromise();
  }

  public async declineExpertOfferAsync(transactionHash: string, scoringId: number, areaId: AreaType) {
    await this.http.put(this.baseApiUrl + '/scoring/offers/reject/', <ChangeStatusExpertOfferRequest>{
      transactionHash: transactionHash,
      scoringId: scoringId,
      areaId: areaId
    }).toPromise();
  }

  public async updateOffersAsync(projectExternalId: string, transactionHash: string): Promise<void> {
    await this.http.put(this.baseApiUrl + '/scoring/offers', <UpdateOffersRequest>{
      projectExternalId: projectExternalId,
      transactionHash: transactionHash
    }).toPromise();
  }

  public async queryAsync(query: OffersQuery): Promise<CollectionResponse<ScoringOfferResponse>> {
    const checkParam = (param) => isNullOrUndefined(param) ? '' : param.toString();

    const parameters = new HttpParams()
      .append('offset', query.offset.toString())
      .append('count', query.count.toString())
      .append('status', checkParam(query.status))
      .append('orderBy', checkParam(query.orderBy))
      .append('sortDirection', checkParam(query.sortDirection));

    return this.http.get<CollectionResponse<ScoringOfferResponse>>(
      `${this.baseApiUrl}/scoring/offers/query`,
      {params: parameters})
      .toPromise();
  }
}
