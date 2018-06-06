import {Injectable} from '@angular/core';
import {StageEnum} from './stage.enum';
import {Country} from './country';
import {Countries} from './countries';
import {Category} from './category';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../project/social-media-type.enum';
import {OfferStatus} from '../../api/scoring-offer/offer-status.enum';


@Injectable()
export class DictionariesService {

  public stages: EnumItem<StageEnum>[] = [];
  public offerStatuses: EnumItem<OfferStatus>[] = [];
  public countries: Country[] = [];
  public categories: EnumItem<Category>[] = [];
  public networks: EnumItem<SocialMediaTypeEnum>[] = [];

  constructor(private translateService: TranslateService) {
  }


  public async initializeAsync(): Promise<void> {
    this.countries = await this.getCountriesAsync();
    this.stages = await this.getStagesAsync();
    this.categories = await this.getCategoriesAsync();
    this.networks = this.getSocialMedias();
    this.offerStatuses = await this.getOfferStatusesAsync();
  }

  public async getOfferStatusesAsync() {
    return await this.enumToTranslatedArray<OfferStatus>(OfferStatus, 'OfferStatuses.');
  }

  public async getCountriesAsync(): Promise<Country[]> {
    const translatedCountries: Country[] = [];
    for (const countryCode in Countries) {
      const currentCountry = {
        name: await this.translateService.get('Countries.' + countryCode).toPromise(),
        code: countryCode
      };
      translatedCountries.push(currentCountry);
    }
    return translatedCountries;
  }

  private getSocialMedias(): EnumItem<SocialMediaTypeEnum>[] {
    return this.enumToArray(SocialMediaTypeEnum);
  }

  private async getStagesAsync() {
    return await this.enumToTranslatedArray<StageEnum>(StageEnum, 'Stages.');
  }

  private async getCategoriesAsync() {
    return await this.enumToTranslatedArray<Category>(Category, 'Categories.');
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
