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

    public getAreasIdByNames(types: string[]) {
        const areasId: number[] = [];
        for (let k = 0; types.length > k; k++) {
            for (let i = 0; this.areas.length > i; i++) {
                if (this.areas[i].name === types[k]) {
                    areasId.push(i);
                }
            }
        }
        return areasId;
    }

    public getAreaTypeByIndex(index: number): AreaType {
        return this.areas[index].areaType;
    }

    public getAreaNameByIndex(index: number): string {
        return this.areas[index].name;
    }
}
