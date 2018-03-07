import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {ExpertScoringOffer} from './expert-scoring-offer';

@Injectable()
export class OffersApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }
  public async getExpertOffersAsync(): Promise<CollectionResponse<ExpertScoringOffer>> {
     return await this.http.get<CollectionResponse<ExpertScoringOffer>>(`${this.baseApiUrl}/scoring/offers/accepted`).toPromise();
  }
}
