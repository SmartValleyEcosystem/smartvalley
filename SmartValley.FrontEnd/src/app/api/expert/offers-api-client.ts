import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {ExpertScoringOffer} from './expert-scoring-offer';
import {AreaType} from '../scoring/area-type.enum';
import {ExpertHistoryOffer} from './expert-history-offer';
import {ScoringOfferStatusResponse} from './scoring-offer-status-response';
import {UpdateOffersRequest} from './update-offers-request';
import {ChangeStatusExpertOfferRequest} from './change-status-expert-offer-request';

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

  public getHistoryOffersListAsync(): Promise<CollectionResponse<ExpertHistoryOffer>> {
    return this.http.get<CollectionResponse<ExpertHistoryOffer>>(`${this.baseApiUrl}/scoring/offers/history`).toPromise();
  }

  public getExpertOffersAsync(): Promise<CollectionResponse<ExpertScoringOffer>> {
    return this.http.get<CollectionResponse<ExpertScoringOffer>>(`${this.baseApiUrl}/scoring/offers/accepted`).toPromise();
  }

  public getExpertPendingOffersAsync(): Promise<CollectionResponse<ExpertScoringOffer>> {
    return this.http.get<CollectionResponse<ExpertScoringOffer>>(`${this.baseApiUrl}/scoring/offers/pending`).toPromise();
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

  public async updateOffersAsync(projectId: string, transactionHash: string): Promise<void> {
    await this.http.put(this.baseApiUrl + '/scoring/offers', <UpdateOffersRequest>{
      projectExternalId: projectId,
      transactionHash: transactionHash
    }).toPromise();
  }
}
