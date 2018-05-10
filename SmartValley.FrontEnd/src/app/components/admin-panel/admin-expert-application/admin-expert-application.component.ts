import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {SelectItem} from 'primeng/api';
import {AreaService} from '../../../services/expert/area.service';
import {Country} from '../../../services/common/country';
import {AdminExpertApplicationData} from './admin-expert-application-data';
import * as moment from 'moment';
import {EnumHelper} from '../../../utils/enum-helper';
import {Area} from '../../../services/expert/area';
import {NotificationsService} from 'angular2-notifications';
import {Paths} from '../../../paths';
import {isNullOrUndefined} from 'util';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';
import {DialogService} from '../../../services/dialog-service';
import {TranslateService} from '@ngx-translate/core';

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
              private router: Router,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private dialogService: DialogService,
              private translateService: TranslateService) {
  }

  public areas: SelectItem[] = [];
  public application: AdminExpertApplicationData;
  public areasToAccept: number[] = [];
  public rejectReason: string;

  async ngOnInit() {
    this.areas = this.areaService.areas.map(area => <SelectItem> {
      label: area.name,
      value: +area.areaType
    });

    const id = this.route.snapshot.paramMap.get('id');
    const response = await this.expertApiClient.getApplicationByIdAsync(+id);
    this.application = <AdminExpertApplicationData>{
      id: response.id,
      address: response.address,
      firstName: response.firstName,
      lastName: response.lastName,
      birthDate: moment(response.birthDate).toDate(),
      sex: this.enumHelper.getSexes().find(d => d.value === response.sex).label,
      country: countries.find(c => c.code === response.countryIsoCode).name,
      city: response.city,
      linkedInLink: response.linkedInLink,
      facebookLink: response.facebookLink,
      cvUrl: response.cvUrl,
      description: response.description,
      why: response.why,
      documentType: this.enumHelper.getDocumentTypes().find(d => d.value === response.documentType).label,
      documentNumber: response.documentNumber,
      scanUrl: response.scanUrl,
      photoUrl: response.photoUrl,
      areaTypes: response.areas,
      areas: response.areas.map(a => {
        return <Area>{
          name: this.areas.find(o => o.value === a).label,
          areaType: a
        };
      })
    };
  }

  public async acceptAsync(): Promise<void> {
    if (this.areasToAccept.length === 0) {
      this.notificationService.error('Error', 'Areas to accept should be selected');
      return;
    }

    const transactionHash = await await this.expertsRegistryContractClient.approveAsync(this.application.address, this.areasToAccept);
    if (transactionHash == null) {
      this.notificationService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Common.TryAgain'));

      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('AdminExpertApplication.Dialog'),
      transactionHash
    );

    await this.expertApiClient.acceptExpertApplicationAsync(this.application.id, this.areasToAccept, transactionHash);

    transactionDialog.close();

    await this.router.navigate([Paths.Admin + '/experts/applications']);
  }

  public async rejectAsync(): Promise<void> {
    if (isNullOrUndefined(this.rejectReason)) {
      this.notificationService.error('Error', 'Enter reason');
      return;
    }

    const transactionHash = await this.expertsRegistryContractClient.rejectAsync(this.application.address);
    if (transactionHash == null) {
      this.notificationService.error(
        this.translateService.instant('Common.Error'),
        this.translateService.instant('Common.TryAgain'));

      return;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('AdminExpertApplication.Dialog'),
      transactionHash
    );

    await this.expertApiClient.rejectExpertApplicationAsync(this.application.id, this.rejectReason, transactionHash);

    transactionDialog.close();

    await this.router.navigate([Paths.Admin + '/experts/applications']);
  }
}
