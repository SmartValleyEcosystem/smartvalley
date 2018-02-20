import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {SelectItem} from 'primeng/api';
import {AreaService} from '../../../services/expert/area.service';
import {Country} from '../../register-expert/country';
import {AdminExpertApplicationData} from './admin-expert-application-data';
import * as moment from 'moment';
import {EnumHelper} from '../../../utils/enum-helper';
import {Area} from '../../../services/expert/area';
import {NotificationsService} from 'angular2-notifications';
import {Paths} from '../../../paths';
import {isNullOrUndefined} from 'util';

const countries = <Country[]>require('../../../../assets/countryList.json');

@Component({
  selector: 'app-admin-expert-application',
  templateUrl: './admin-expert-application.component.html',
  styleUrls: ['./admin-expert-application.component.css']
})
export class AdminExpertApplicationComponent implements OnInit {

  constructor(private route: ActivatedRoute,
              private expertApiClient: ExpertApiClient,
              private areaService: AreaService,
              private enumHelper: EnumHelper,
              private notificationService: NotificationsService,
              private router: Router) {
  }

  public areas: SelectItem[] = [];
  public application: AdminExpertApplicationData;
  public areasToAccept: number[] = [];
  public rejectReason: string;

  async ngOnInit() {
    for (const area of this.areaService.areas) {
      this.areas.push({
        label: area.name,
        value: +area.areaType
      });
    }
    const id = this.route.snapshot.paramMap.get('id');
    const response = await this.expertApiClient.getApplicationByIdAsync(+id);
    this.application = <AdminExpertApplicationData>{
      id: response.id,
      firstName: response.firstName,
      lastName: response.lastName,
      birthDate: moment(response.birthDate).toDate(),
      sex: this.enumHelper.getSexes().find(d => d.value === response.sex).label,
      country: countries.find(c => c.code === response.countryIsoCode).name,
      city: response.city,
      linkedInLink: response.linkedInLink,
      facebookLink: response.facebookLink,
      cvName: response.cvName,
      description: response.description,
      why: response.why,
      documentType: this.enumHelper.getDocumentTypes().find(d => d.value === response.documentType).label,
      documentNumber: response.documentNumber,
      scanName: response.scanName,
      photoName: response.photoName,
      areaTypes: response.areas,
      areas: response.areas.map(a => {
        return <Area>{
          name: this.areas.find(o => o.value === a).label,
          areaType: a
        };
      })
    };
  }

  public async accept() {
    if (this.areasToAccept.length === 0) {
      this.notificationService.error('Error', 'Areas to accept should be selected');
      return;
    }
    await this.expertApiClient.acceptExpertApplicationAsync(this.application.id, this.areasToAccept);
    this.router.navigate([Paths.Admin]);
  }


  public async reject() {
    if (isNullOrUndefined(this.rejectReason)) {
      this.notificationService.error('Error', 'Enter reason');
      return;
    }
    await this.expertApiClient.rejectExpertApplicationAsync(this.application.id, this.rejectReason);
    this.router.navigate([Paths.Admin]);
  }
}
