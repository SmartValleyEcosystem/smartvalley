import {Injectable} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {Area} from './area';
import {AreaType} from '../../api/scoring/area-type.enum';

@Injectable()
export class AreaService {

  public areas: Area[];

  constructor(private expertApiClient: ExpertApiClient) {
  }

  public async initializeAsync() {
    const response = await this.expertApiClient.getAreasAsync();
    this.areas = response.items.map(a => {
      return <Area>{
        name: a.name,
        areaType: a.id,
        maxScore: a.maxScore
      };
    });
  }

  public getNameByType(areaType: AreaType): string {
    return this.areas.find(a => a.areaType === areaType).name;
  }

  public getMaxScore(areaType: AreaType): number {
    return this.areas.find(a => a.areaType === areaType).maxScore;
  }
}
