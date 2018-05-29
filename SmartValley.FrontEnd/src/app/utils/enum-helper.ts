import {SelectItem} from 'primeng/api';
import {Injectable} from '@angular/core';
import {DocumentEnum} from '../components/register-expert/document.enum';
import {TranslateService} from '@ngx-translate/core';
import {SexEnum} from '../components/register-expert/sex.enum';

@Injectable()
export class EnumHelper {

  constructor(private translateService: TranslateService) {
  }

  public getDocumentTypes(): SelectItem[] {
    return [
      {label: this.translateService.instant('Passport'), value: DocumentEnum.Passport},
      {label: this.translateService.instant('Driver License'), value: DocumentEnum.DriverLicense},
      {label: this.translateService.instant('Id'), value: DocumentEnum.Id},
      {label: this.translateService.instant('Other'), value: DocumentEnum.Other}
    ];
  }

  public getSexes(): SelectItem[] {
    return [
      {label: this.translateService.instant('Male'), value: SexEnum.Male},
      {label: this.translateService.instant('Female'), value: SexEnum.Female},
      {label: this.translateService.instant('Not specified'), value: SexEnum.NotSpecified}
    ];
  }
}
