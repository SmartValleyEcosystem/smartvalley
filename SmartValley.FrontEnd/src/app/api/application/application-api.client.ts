import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BaseApiClient} from '../base-api-client';
import {SubmitApplicationRequest} from './submit-application-request';
import {CollectionResponse} from '../collection-response';
import {CountryResponse} from './country-response';
import {CategoryResponse} from './category-response';
import {Countries} from './countries';
import {Country} from '../../components/register-expert/country';
import {TranslateService} from '@ngx-translate/core';

@Injectable()
export class ApplicationApiClient extends BaseApiClient {
  constructor(private http: HttpClient,
              private translateService: TranslateService) {
    super();
  }

  public async submitAsync(request: SubmitApplicationRequest): Promise<void> {
    await this.http.post(this.baseApiUrl + '/applications', request).toPromise();
  }

  public getCountries(): Country[] {
    let tranlatedCountries: Country[] = [];
    for (let countryCode in Countries) {
        const currentCountry = {
          name: this.translateService.instant('Countries.' + countryCode),
          code: countryCode
        };
        tranlatedCountries.push(currentCountry);
    }
    return tranlatedCountries;
  }

  public getCategoriesAsync(): Promise<CollectionResponse<CategoryResponse>> {
    return this.http.get<CollectionResponse<CategoryResponse>>(`${this.baseApiUrl}/applications/categories`).toPromise();
  }
}
