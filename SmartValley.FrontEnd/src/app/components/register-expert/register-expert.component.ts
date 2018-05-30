import {Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren} from '@angular/core';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {CreateExpertApplicationRequest} from '../../api/expert/expert-applitacion-request';
import {ExpertsRegistryContractClient} from '../../services/contract-clients/experts-registry-contract-client';
import {AreaType} from '../../api/scoring/area-type.enum';
import {DialogService} from '../../services/dialog-service';
import {SexEnum} from './sex.enum';
import {DocumentEnum} from './document.enum';
import {Country} from '../../services/common/country';
import {TranslateService} from '@ngx-translate/core';
import {SelectItem} from 'primeng/api';
import * as moment from 'moment';
import {NotificationsService} from 'angular2-notifications';
import {AreaService} from '../../services/expert/area.service';
import {EnumHelper} from '../../utils/enum-helper';
import {Md5} from 'ts-md5';
import {FileUploaderHelper} from '../../utils/file-uploader-helper';

const countries = <Country[]>require('../../../assets/countryList.json');

@Component({
  selector: 'app-register-expert',
  templateUrl: './register-expert.component.html',
  styleUrls: ['./register-expert.component.css']
})
export class RegisterExpertComponent implements OnInit {

  public registryForm: FormGroup;
  public isSubmitting: boolean;

  public sex: SelectItem[];
  public documentTypes: SelectItem[];
  public countries: SelectItem[];
  public areas: SelectItem[];
  public selectedAreas: AreaType[] = [];
  public isAreasSelected = true;

  @ViewChildren('required') public requiredFields: QueryList<any>;
  @ViewChild('areasBlock') private areasBlock: ElementRef;

  private cv: File;
  private document: File;
  private photo: File;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private expertApiClient: ExpertApiClient,
              private dialogService: DialogService,
              private notificationsService: NotificationsService,
              private translateService: TranslateService,
              private expertsRegistryContractClient: ExpertsRegistryContractClient,
              private areaService: AreaService,
              private enumHelper: EnumHelper) {
  }

  public ngOnInit(): void {
    this.createForm();
  }

  private createForm() {
    this.sex = this.enumHelper.getSexes();
    this.documentTypes = this.enumHelper.getDocumentTypes();

    this.areas = this.areaService.areas.map(a => <SelectItem> {
      label: a.name,
      value: +a.areaType
    });

    this.countries = [];

    for (const item of countries) {
      this.countries.push({label: item.name, value: item});
    }

    this.registryForm = this.formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      secondName: ['', [Validators.required, Validators.maxLength(50)]],
      linkedin: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      facebook: ['', [Validators.maxLength(400), Validators.pattern('https?://.+')]],
      why: ['', [Validators.required, Validators.maxLength(1500)]],
      description: ['', [Validators.maxLength(1500)]],
      country: ['', [Validators.required]],
      document: ['', [Validators.required]],
      photo: ['', [Validators.required]],
      city: ['', [Validators.required, Validators.maxLength(50)]],
      selectedSex: [''],
      selectedDocumentType: [DocumentEnum.Passport],
      birthDate: ['', [Validators.required, Validators.maxLength(100)]],
      number: ['', [Validators.required, Validators.maxLength(30)]],
      documentTypes: [this.documentTypes],
    });
  }


  public async applyAsync(): Promise<void> {

    if (!this.validateForm()) {
      if (this.isAreasCheckboxesValid()) {
        return;
      }
      return;
    }

    this.isSubmitting = true;

    const isSucceeded = await this.submitAsync();
    if (isSucceeded) {
      await this.router.navigate([Paths.ExpertStatus]);
    }
    this.isSubmitting = false;
  }

  public onCvUpload(event: any) {
    const element = this.requiredFields.find(f => f.name === 'cv');
    if (event !== null) {
      this.cv = event.files[0];
      if (FileUploaderHelper.checkCVExtensions(this.cv)) {
        this.switchFileUploadValidity(element, true);
      } else {
        this.switchFileUploadValidity(element, false);
        this.notificationsService.error(this.translateService.instant('RegisterExpert.CVFormatError'));
      }
    } else {
      this.cv = null;
      this.switchFileUploadValidity(element, false);
    }
  }

  public onPhotoSizeError() {
    this.notificationsService.error(this.translateService.instant('RegisterExpert.PhotoSizeError'));
  }

  public onDocumentSizeError() {
    this.notificationsService.error(this.translateService.instant('RegisterExpert.DocumentSizeError'));
  }

  private switchFileUploadValidity(element: any, isValid: boolean) {
    if (isValid) {
      element.el.nativeElement.classList.remove('ng-invalid');
      element.el.nativeElement.classList.remove('ng-dirty');
      element.el.nativeElement.classList.add('ng-valid');
    } else {
      element.el.nativeElement.classList.remove('ng-valid');
      element.el.nativeElement.classList.add('ng-invalid');
      element.el.nativeElement.classList.add('ng-dirty');
    }
  }

  private setInvalid(element: ElementRef) {
    if (element.nativeElement.nativeElement) {
      element.nativeElement.nativeElement.classList.add('ng-invalid');
      element.nativeElement.nativeElement.classList.add('ng-dirty');
      return;
    }
    if (element.nativeElement) {
      element.nativeElement.classList.add('ng-invalid');
      element.nativeElement.classList.add('ng-dirty');
    }
  }

  private validateForm(): boolean {
    if (!this.registryForm.invalid
      && this.cv != null
      && FileUploaderHelper.checkCVExtensions(this.cv)) {
      return true;
    }

    const invalidElements = this.requiredFields.filter(
      (i) => {
        let elem = false;

        if (i.el) {
          elem = i.el.nativeElement.classList.contains('ng-invalid');
          if (i.files && i.files.length === 0) {
            elem = i;
          }
        }

        if (i.nativeElement) {
          if (i.nativeElement.nativeElement) {
            return i.nativeElement.nativeElement.classList.contains('ng-invalid');
          }
          elem = i.nativeElement.classList.contains('ng-invalid');
        }

        return elem;
      });

    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        const element = invalidElements[a].el !== undefined ? invalidElements[a].el : invalidElements[a];
        this.setInvalid(element);
      }
      const firstElement = invalidElements[0].el !== undefined ? invalidElements[0].el : invalidElements[0];
      this.scrollToElement(firstElement);
    }
    return false;
  }

  private scrollToElement(element: ElementRef) {
    if (element.nativeElement.nativeElement) {
      const offsetTop1 = element.nativeElement.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
      return;
    }
    if (element.nativeElement) {
      const offsetTop1 = element.nativeElement.offsetTop;
      window.scrollTo({left: 0, top: offsetTop1 - 40, behavior: 'smooth'});
      return;
    }
  }

  private createExpertApplicationRequest(transactionHash: string, areas: Array<AreaType>): CreateExpertApplicationRequest {
    const form = this.registryForm.value;
    const input = new FormData();
    input.append('scan', form.document);
    input.append('photo', form.photo);
    input.append('cv', this.cv);
    input.append('transactionHash', transactionHash);
    input.append('sex', form.selectedSex === '' ? SexEnum.NotSpecified.toString() :
      (form.selectedSex ? SexEnum.Male.toString() : SexEnum.Female.toString()));
    input.append('birthDate', moment(form.birthDate).toISOString());
    input.append('city', form.city);
    input.append('countryIsoCode', form.country.code);
    input.append('documentNumber', form.number);
    input.append('documentType', form.selectedDocumentType);
    input.append('facebookLink', form.facebook);
    input.append('linkedInLink', form.linkedin);
    input.append('firstName', form.firstName);
    input.append('lastName', form.secondName);
    input.append('description', form.description);
    input.append('why', form.why);
    areas.forEach(a => input.append('areas', a.toString()));

    return <CreateExpertApplicationRequest>{
      body: input
    };
  }

  private async submitAsync(): Promise<boolean> {
    const areas = this.selectedAreas.map(a => +a);
    if (areas.length === 0) {
      this.notificationsService.error(this.translateService.instant('RegisterExpert.CategoryNotSelectedError'));
      return false;
    }

    const applicationHash = this.getApplicationHash(areas);
    const transactionHash = await this.applyToContractAsync(areas, applicationHash);
    if (transactionHash == null) {
      return false;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('RegisterExpert.Dialog'),
      transactionHash
    );

    const request = this.createExpertApplicationRequest(transactionHash, areas);

    await this.expertApiClient.createApplicationAsync(request);

    transactionDialog.close();
    return true;
  }

  private async applyToContractAsync(areas: Array<AreaType>, applicationHash: string): Promise<string> {
    try {
      return await this.expertsRegistryContractClient.applyAsync(areas, applicationHash);
    } catch (e) {
      return null;
    }
  }

  private getApplicationHash(areas: Array<AreaType>): string {
    const form = this.registryForm.value;
    const applicationStr = (+form.selectedSex) +
      moment(form.birthDate).toISOString() +
      form.city +
      form.country.code +
      form.number +
      form.selectedDocumentType +
      form.facebook +
      form.linkedIn +
      form.firstName +
      form.secondName +
      form.description +
      form.why;

    areas.forEach(a => applicationStr.concat(a.toString()));
    return '0x' + Md5.hashStr(applicationStr, false).toString();
  }

  public isAreasCheckboxesValid() {
    this.isAreasSelected = true;
    const areas = this.selectedAreas.map(a => +a);
    if (!areas.length) {
      this.isAreasSelected = false;
      this.scrollToElement(this.areasBlock);
      return true;
    }
    return false;
  }
}
