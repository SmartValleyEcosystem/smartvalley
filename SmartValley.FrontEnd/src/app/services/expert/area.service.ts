import {Injectable} from '@angular/core';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {Area} from './area';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';

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

    public getAreasByTypes(types: any[]): Area[] {
        return types.map(a => a.name);
    }

    public getAreasIdByTypes(types: string[]) {
        const areasId: number[] = [];
        for (let k = 0; types.length > k; k++) {
            for (let i = 0; this.areas.length > i; i++) {
                if (this.areas[i].name === types[k]) {
                    areasId.push(i);
                }
            }
        };
        return areasId;
    }
}
