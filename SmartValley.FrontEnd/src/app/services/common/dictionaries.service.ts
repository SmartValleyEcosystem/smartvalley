import {Injectable} from '@angular/core';
import {StageEnum} from './stage.enum';
import {Country} from './country';
import {Countries} from './countries';
import {CategoryEnum} from './category.enum';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../project/social-media-type.enum';


@Injectable()
export class DictionariesService {

  public stages: EnumItem<StageEnum>[] = [];
  public countries: Country[] = [];
  public categories: EnumItem<CategoryEnum>[] = [];
  public networks: EnumItem<SocialMediaTypeEnum>[] = [];

  constructor(private translateService: TranslateService) {
  }


  public async initializeAsync(): Promise<void> {
    this.countries = await this.getCountriesAsync();
    this.stages = await this.getStagesAsync();
    this.categories = await this.getCategoriesAsync();
    this.networks = this.getSocialMedias();
  }

  public async getCountriesAsync(): Promise<Country[]> {
    const tranlatedCountries: Country[] = [];
    for (const countryCode in Countries) {
      const currentCountry = {
        name: await this.translateService.get('Countries.' + countryCode).toPromise(),
        code: countryCode
      };
      tranlatedCountries.push(currentCountry);
    }
    return tranlatedCountries;
  }

  private getSocialMedias(): EnumItem<SocialMediaTypeEnum>[] {
    return this.enumToArray(SocialMediaTypeEnum);
  }

  private async getStagesAsync() {
    return await this.enumToTranslatedArray<StageEnum>(StageEnum, 'Stages.');
  }

  private async getCategoriesAsync() {
    return await this.enumToTranslatedArray<CategoryEnum>(CategoryEnum, 'Categories.');
  }

  private async enumToTranslatedArray<E>(Enum: any, translationCategory: string): Promise<EnumItem<E>[]> {
    const items = Object.keys(Enum)
      .filter(value => isNaN(+value));
    
    const enumItems = [];
    for (const item of items) {
      const id = Enum[item];
      const value = await this.translateService.get(translationCategory + Enum[item]).toPromise();
      enumItems.push(<EnumItem<E>>{
        id: id,
        value: value
      });
    }

    return enumItems as EnumItem<E>[];
  }

  private enumToArray<E>(Enum: any): EnumItem<E>[] {
    return Object.keys(Enum)
      .filter(value => isNaN(+value))
      .map(value => ({
        id: Enum[value],
        value: value
      } as EnumItem<E>));
  }
}
