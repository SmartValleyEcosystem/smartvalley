import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AddAdminModalComponent} from '../add-admin-modal/add-admin-modal.component';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {NewExpertRequest} from '../../../api/expert/new-expert-request';
import {ExpertContractClient} from '../../../services/contract-clients/expert-contract-client';
import {UserApiClient} from '../../../api/user/user-api-client';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';

@Component({
    selector: 'app-create-new-expert-modal.component',
    templateUrl: './create-new-expert-modal.component.html',
    styleUrls: ['./create-new-expert-modal.component.css']
})
export class CreateNewExpertModalComponent implements OnInit {

    public form: FormGroup;
    public newExpertResponse: any;
    public newExpertRequest: NewExpertRequest;
    public transactionHash: string;
    public selectedCategories: number [] = [];
    public email: string;

    constructor(private expertApiClient: ExpertApiClient,
                private userApiClient: UserApiClient,
                private formBuilder: FormBuilder,
                @Inject(MAT_DIALOG_DATA) public data: any,
                private dialogCreateExpert: MatDialogRef<CreateNewExpertModalComponent>,
                private notificationsService: NotificationsService,
                private expertContractClient: ExpertContractClient,
                private translateService: TranslateService) {
    }
    async ngOnInit() {
        this.form = this.formBuilder.group({
            address: ['', [Validators.required, AddAdminModalComponent.validateWalletAddress]],
            available: [''],
            category1: [''],
            category2: [''],
            category3: [''],
            category4: [''],
            category5: ['']
        });
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
                    this.selectedCategories.push( parseInt(control.match(/category([0-9].*)/)[1]) );
                }
            }
        }

        this.transactionHash = ( await this.expertContractClient.addSync(form.value.address, [1]) );
        this.newExpertRequest = {
            transactionHash: this.transactionHash,
            address: form.value.address,
            email: this.email,
            name: '',
            about: '',
            isAvailable: form.value.available || false,
            areas: this.selectedCategories
        };

        this.newExpertResponse = (await this.expertApiClient.createNewExpertsAsync(this.newExpertRequest));
        this.dialogCreateExpert.close();
    }
}
