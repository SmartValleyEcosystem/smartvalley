import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import {UserApiClient} from '../../../api/user/user-api-client';
import {AreaService} from '../../../services/expert/area.service';
import {FormGroup, Validators, FormBuilder} from '@angular/forms';
import {ExpertContractClient} from '../../../services/contract-clients/expert-contract-client';
import {EditExpertRequest} from '../../../api/expert/edit-expert-request';

@Component({
  selector: 'app-edit-expert-modal',
  templateUrl: './edit-expert-modal.component.html',
  styleUrls: ['./edit-expert-modal.component.css']
})
export class EditExpertModalComponent implements OnInit {

    public selectedCategories: number[];
    public categoriesModel = true;
    public saveBackendForm: FormGroup;
    public saveBlockchainForm: FormGroup;
    public editExpertRequest: EditExpertRequest;

    constructor(private expertApiClient: ExpertApiClient,
                private userApiClient: UserApiClient,
                @Inject(MAT_DIALOG_DATA) public data: any,
                private areaService: AreaService,
                private formBuilder: FormBuilder,
                private dialogCreateExpert: MatDialogRef<EditExpertModalComponent>,
                private expertContractClient: ExpertContractClient) {
    }

    ngOnInit() {
        this.saveBackendForm = this.formBuilder.group({
            address: ['', [Validators.required, Validators.minLength(8)]],
            email: ['', Validators.required],
            name: ['', Validators.required],
            about: ['', Validators.required]
        });

        this.saveBlockchainForm = this.formBuilder.group({
            available: [''],
            category1: [''],
            category2: [''],
            category3: [''],
            category4: [''],
            category5: ['']
        });
        this.selectedCategories = this.areaService.getAreasIdByTypes(this.data.areas);
    }

    public isCategotySelected(id) {
      return this.selectedCategories.includes(id);
    }

    public categoryChange(event) {
      const categoryId =  parseInt(event.source.name.match(/category([0-9].*)/)[1]);
      if (event.checked && !this.selectedCategories.includes(categoryId)) {
          this.selectedCategories.push(categoryId);
          return;
      }
      this.selectedCategories = this.selectedCategories.filter(e => e !== categoryId);
    }

    public SaveBlockchain() {
        this.dialogCreateExpert.close();
    }

    async submit(form, blockChainRequest = true) {
        const address = this.saveBackendForm.value.address;
        const email = this.saveBackendForm.value.email;
        const name = this.saveBackendForm.value.name;
        const about = this.saveBackendForm.value.about;
        const isAvailable = this.saveBlockchainForm.value.available || false;

        this.editExpertRequest = {
            address: address,
            email: email,
            name: name,
            about: about,
            isAvailable: isAvailable,
            areas: this.selectedCategories
        };

        if (blockChainRequest) {
            const transactionHash = ( await this.expertContractClient.addSync(address, this.selectedCategories) );
            this.editExpertRequest.transactionHash = transactionHash;
        }

        await this.expertApiClient.editExpertAsync(this.editExpertRequest);
        this.dialogCreateExpert.close();
    }
}
