import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {ExpertRequest} from '../../../api/expert/expert-request';
import {UserApiClient} from '../../../api/user/user-api-client';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';
import {AreaService} from '../../../services/expert/area.service';
import {Area} from '../../../services/expert/area';
import {ExpertsRegistryContractClient} from '../../../services/contract-clients/experts-registry-contract-client';

@Component({
    selector: 'app-create-new-expert-modal.component',
    templateUrl: './create-new-expert-modal.component.html',
    styleUrls: ['./create-new-expert-modal.component.css']
})
export class CreateNewExpertModalComponent implements OnInit {

    public form: FormGroup;
    public newExpertResponse: any;
    public newExpertRequest: ExpertRequest;
    public transactionHash: string;
    public selectedCategories: number [] = [];
    public email: string;
    public areas: Area[];

    constructor(private expertApiClient: ExpertApiClient,
                private userApiClient: UserApiClient,
                private formBuilder: FormBuilder,
                @Inject(MAT_DIALOG_DATA) public data: any,
                private dialogCreateExpert: MatDialogRef<CreateNewExpertModalComponent>,
                private notificationsService: NotificationsService,
                private expertsRegistryContractClient: ExpertsRegistryContractClient,
                private translateService: TranslateService,
                private areaService: AreaService) {
    }
    async ngOnInit() {
        this.areas = this.areaService.areas;
        this.selectedCategories = [];
        let formGroupInputs = {
            address: ['', [Validators.required, AddAdminModalComponent.validateWalletAddress]],
            available: [''],
        };
        const categoryFormElements = this.areas.map((a, i, ar) => {
            const currentField: {} = {};
            currentField['category' + a.areaType] = [''];
            formGroupInputs = Object.assign(formGroupInputs, currentField);
        });
        this.form = this.formBuilder.group(formGroupInputs);
    }

    async submit(form) {
        const user = await this.userApiClient.getByAddressAsync(form.value.address);
        this.email = user.email;

        if (user.address == null) {
            this.notificationsService.error(
                this.translateService.instant('Common.Error'),
                this.translateService.instant('Common.UserNotFound'));
            return;
        }

        for ( const control in form.controls ) {
            if ( (/category/i).test(control) ) {
                if ( form.value[control] ) {
                    this.selectedCategories.push( parseInt(control.match(/category([0-9].*)/)[1], 0) );
                }
            }
        }
        this.transactionHash = ( await this.expertsRegistryContractClient.addAsync(form.value.address, this.selectedCategories) );
        this.newExpertRequest = {
            transactionHash: this.transactionHash,
            address: form.value.address,
            email: this.email,
            name: '',
            about: '',
            isAvailable: form.value.available || false,
            areas: this.selectedCategories
        };

        this.newExpertResponse = await this.expertApiClient.createAsync(this.newExpertRequest);
        this.dialogCreateExpert.close();
    }
}
