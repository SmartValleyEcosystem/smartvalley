import {Injectable} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {Area} from './area';

@Injectable()
export class AreaService {

  constructor(private expertApiClient: ExpertApiClient) {
  }

  public areas: Area[];

  public async initializeAsync() {
    const response = await this.expertApiClient.getAreasAsync();
    this.areas = response.items.map(a => {
      return <Area>{
        name: a.name,
        areaType: a.id
      };
    });
  }
}
