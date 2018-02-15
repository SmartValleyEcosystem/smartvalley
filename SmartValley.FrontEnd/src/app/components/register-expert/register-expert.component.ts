import {Component, ElementRef, OnInit, QueryList, ViewChildren} from '@angular/core';
import {Paths} from '../../paths';
import {Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {ExpertApplicationRequest} from '../../api/expert/expert-applitacion-request';
import {ExpertContractClient} from '../../services/contract-clients/expert-contract-client';
import {ExpertiseArea} from '../../api/scoring/expertise-area.enum';
import {DialogService} from '../../services/dialog-service';
import {SexEnum} from './sex.enum';
import {DocumentEnum} from './document.enum';
import {Country} from './country';
import {TranslateService} from '@ngx-translate/core';
import {SelectItem} from 'primeng/api';
import {UserContext} from '../../services/authentication/user-context';

const countries = <Country[]>require('../../../assets/countryList.json');

@Component({
  selector: 'app-register-expert',
  templateUrl: './register-expert.component.html',
  styleUrls: ['./register-expert.component.css']
})
export class RegisterExpertComponent implements OnInit {

  public registryForm: FormGroup;
  public isLetterShow = false;
  public isKYCShow = false;
  public isProjectCreating: boolean;

  public sex: SelectItem[];
  public documentTypes: SelectItem[];
  public countries: SelectItem[];
  public areas: SelectItem[];
  public selectedAreas: ExpertiseArea[] = [];

  @ViewChildren('required') public requiredFields: QueryList<any>;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private expertApiClient: ExpertApiClient,
              private dialogService: DialogService,
              private userContext: UserContext,
              private translateService: TranslateService,
              private expertContactClient: ExpertContractClient) {
  }

  public ngOnInit(): void {
    this.createForm();
  }

  public showNext() {
    if (this.isLetterShow === false) {
      this.isLetterShow = true;
      return;
    }
    if (this.isKYCShow === false) {
      this.isKYCShow = true;
      return;
    }
  }

  private createForm() {
    this.sex = [
      {label: 'Male', value: SexEnum.Male},
      {label: 'Female', value: SexEnum.Female}
    ];

    this.documentTypes = [
      {label: 'Passport', value: DocumentEnum.Passport},
      {label: 'DriverLicense', value: DocumentEnum.DriverLicense},
      {label: 'Id', value: DocumentEnum.Id},
      {label: 'Other', value: DocumentEnum.Other}
    ];

    this.areas = [
      {label: 'TechnicalExpert', value: ExpertiseArea.TechnicalExpert},
      {label: 'Lawyer', value: ExpertiseArea.Lawyer},
      {label: 'HR', value: ExpertiseArea.HR},
      {label: 'Analyst', value: ExpertiseArea.Analyst}
    ];

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
      description: ['', [Validators.required, Validators.maxLength(1500)]],
      country: [this.countries[0]],
      city: ['', Validators.maxLength(100)],
      selectedSex: [SexEnum.Male],
      selectedDocumentType: [DocumentEnum.Passport],
      birthDate: ['', Validators.maxLength(100)],
      number: ['', Validators.maxLength(100)],
      document: ['', Validators.maxLength(100)],
      photo: ['', Validators.maxLength(100)]
    });
  }

  public async applyAsync(): Promise<void> {
    if (!this.validateForm()) {
      return;
    }

    const isSucceeded = await this.submitAsync();
    if (isSucceeded) {
      await this.router.navigate([Paths.Root]);
    }
  }

  private setInvalid(element: ElementRef) {
    element.nativeElement.classList.add('ng-invalid');
    element.nativeElement.classList.add('ng-dirty');
  }

  private validateForm(): boolean {
    if (!this.registryForm.invalid) {
      return true;
    }

    const invalidElements = this.requiredFields.filter(i => i.nativeElement.classList.contains('ng-invalid'));
    if (invalidElements.length > 0) {
      for (let a = 0; a < invalidElements.length; a++) {
        this.setInvalid(invalidElements[a]);
      }
      this.scrollToElement(invalidElements[0]);
    }
    return false;
  }

  private scrollToElement(element: ElementRef) {
    const offsetTop1 = element.nativeElement.offsetTop;
    const offsetTop3 = element.nativeElement.offsetParent.offsetParent.offsetTop;
    window.scrollTo({left: 0, top: offsetTop1 + offsetTop3, behavior: 'smooth'});
  }

  private getAreas(): Array<ExpertiseArea> {
    return this.selectedAreas;
  }

  private createExpertApplicationRequest(transactionHash: string, areas: Array<ExpertiseArea>): ExpertApplicationRequest {
    const user = this.userContext.getCurrentUser();
    const form = this.registryForm.value;
    return <ExpertApplicationRequest>{
      transactionHash: transactionHash,
      sex: <number>form.selectedSex,
      applicantAddress: user.account,
      birthDate: <Date>form.birthDate,
      city: form.city,
      countryIsoCode: form.country.value.code,
      documentNumber: form.number,
      documentType: form.selectedDocumentType,
      facebookLink: form.facebook,
      linkedInLink: form.linkedin,
      firstName: form.firstName,
      lastName: form.secondName,
      description: form.description,
      why: form.why,
      areas: areas
    };
  }

  private async submitAsync(): Promise<boolean> {
    const areas = this.getAreas();

    const transactionHash = await this.applyToContractAsync(areas);
    if (transactionHash == null) {
      return false;
    }

    const transactionDialog = this.dialogService.showTransactionDialog(
      this.translateService.instant('Estimate.Dialog'),
      transactionHash
    );

    const request = this.createExpertApplicationRequest(transactionHash, areas);

    await this.expertApiClient.applyAsync(request);

    transactionDialog.close();
    return true;
  }

  private async applyToContractAsync(areas: Array<ExpertiseArea>): Promise<string> {
    try {
      return await this.expertContactClient.applyAsync(areas);
    } catch (e) {
      return null;
    }
  }
}
