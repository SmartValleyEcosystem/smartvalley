import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {ExpertScoringOffer} from './expert-scoring-offer';
import {ExpertHistoryOffer} from './expert-history-offer';

@Injectable()
export class OffersApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }
  public async getExpertOffersAsync(): Promise<CollectionResponse<ExpertScoringOffer>> {
     return await this.http.get<CollectionResponse<ExpertScoringOffer>>(`${this.baseApiUrl}/scoring/offers/accepted`).toPromise();
  }
  public async getHistoryOffersListAsync(): Promise<CollectionResponse<ExpertHistoryOffer>> {
     return await this.http.get<CollectionResponse<ExpertHistoryOffer>>(`${this.baseApiUrl}/scoring/offers/history`).toPromise();
  }
}
