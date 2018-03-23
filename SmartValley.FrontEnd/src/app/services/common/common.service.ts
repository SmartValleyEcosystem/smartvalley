import {Injectable} from '@angular/core';
import {StageEnum} from './stage.enum';
import {Country} from './country';
import {Category} from './category';
import {Stage} from './stage';
import {Countries} from './countries';
import {CategoryEnum} from './category.enum';
import {TranslateService} from '@ngx-translate/core';
import {SocialMediaTypeEnum} from '../project/social-media-type.enum';
import {SocialMedia} from '../project/social-media';

@Injectable()
export class CommonService {

  public stages: Stage[] = [];
  public countries: Country[] = [];
  public categories: Category[] = [];
  public networks: SocialMedia[] = [];

  constructor(private translateService: TranslateService) {
  }


  public initialize(): void {
    this.countries = this.getCountries();
    this.stages = this.getStages();
    this.categories = this.getCategories();
    this.networks = this.getSocialMedias();
  }

  private getCountries(): Country[] {
    const tranlatedCountries: Country[] = [];
    for (const countryCode in Countries) {
      const currentCountry = {
        name: this.translateService.instant('Countries.' + countryCode),
        code: countryCode
      };
      tranlatedCountries.push(currentCountry);
    }
    return tranlatedCountries;
  }

  private getSocialMedias(): Array<SocialMedia> {
    return <Array<SocialMedia>>[
      {name: 'All socials', socialMediaType: null},
      {name: 'Bitcoin Talk', socialMediaType: SocialMediaTypeEnum.BitcoinTalk},
      {name: 'Facebook', socialMediaType: SocialMediaTypeEnum.Facebook},
      {name: 'Github', socialMediaType: SocialMediaTypeEnum.Github},
      {name: 'LinkedIn', socialMediaType: SocialMediaTypeEnum.LinkedIn},
      {name: 'Medium', socialMediaType: SocialMediaTypeEnum.Medium},
      {name: 'Reddit', socialMediaType: SocialMediaTypeEnum.Reddit},
      {name: 'Telegram', socialMediaType: SocialMediaTypeEnum.Telegram},
      {name: 'Twitter', socialMediaType: SocialMediaTypeEnum.Twitter}
    ];
  }

  private getStages(): Stage[] {
    const stages = Object.keys(StageEnum).filter(key => !isNaN(Number(StageEnum[key])));
    return stages.map(stageId => <Stage> {
        name: this.translateService.instant('Stages.' + StageEnum[stageId]),
        type: Number.parseInt(stageId, 0)
      }
    );
  }

  private getCategories(): Category[] {
    const categories = Object.keys(CategoryEnum).filter(key => !isNaN(Number(CategoryEnum[key])));
    return categories.map(categoryId => <Category> {
        name: this.translateService.instant('Categories.' + CategoryEnum[categoryId]),
        type: Number.parseInt(categoryId, 0)
      }
    );
  }
}
