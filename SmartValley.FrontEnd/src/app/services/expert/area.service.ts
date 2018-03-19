import {Injectable} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {Area} from './area';
import {AreaType} from '../../api/scoring/area-type.enum';

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

    public getAreasByTypes(types: Area[]): string[] {
        return types.map(a => a.name);
    }

    public getAreaTypeByIndex(index: number): AreaType {
        return this.areas[index].areaType;
    }
}
