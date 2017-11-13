import {environment} from '../../environments/environment';

export class BaseApiClient {
  protected baseApiUrl = environment.baseUrl + '/api';
}
