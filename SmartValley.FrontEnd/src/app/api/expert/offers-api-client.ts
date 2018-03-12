import {HttpClient, HttpParams} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {CollectionResponse} from '../collection-response';
import {ExpertScoringOffer} from './expert-scoring-offer';
import {ScoringOfferStatusResponse} from './scoring-offer-status-response';

@Injectable()
export class OffersApiClient extends BaseApiClient {
  constructor(private http: HttpClient) {
    super();
  }

  public getExpertOffersAsync(): Promise<CollectionResponse<ExpertScoringOffer>> {
    return this.http.get<CollectionResponse<ExpertScoringOffer>>(`${this.baseApiUrl}/scoring/offers/accepted`).toPromise();
  }

  public getOfferStatusAsync(projectId: number, areaType: number): Promise<ScoringOfferStatusResponse> {
    const parameters = new HttpParams()
      .append('projectId', projectId.toString())
      .append('areaType', areaType.toString());

    return this.http.get<ScoringOfferStatusResponse>(`${this.baseApiUrl}/scoring/offers/status`, {
      params: parameters
    }).toPromise();
  }
}
