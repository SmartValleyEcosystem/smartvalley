import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';

@Component({
    selector: 'app-new-allotment-event-modal',
    templateUrl: './new-allotment-event-modal.component.html',
    styleUrls: ['./new-allotment-event-modal.component.scss']
})
export class NewAllotmentEventModalComponent implements OnInit {
    public form: FormGroup;
    public allowedProjects: SelectItem[] = [];

    constructor(private formBuilder: FormBuilder,
                private allotmentEventService: AllotmentEventService,
                private dialogRef: MatDialogRef<NewAllotmentEventModalComponent>,
                private projectApiClient: ProjectApiClient) {
    }

    async ngOnInit() {
        this.form = this.formBuilder.group({
            project: ['', [Validators.required]],
            eventName: ['', [Validators.required]],
            tokenAddress: ['', [Validators.required]],
            ticker: ['', [Validators.required]],
            tokenDecimals: ['', [Validators.required]],
            finishDate: ['', [Validators.required]],
        });

        const myProjectResponse = await this.projectApiClient.getMyProjectAsync();
        this.allowedProjects.push({
            label: myProjectResponse.name,
            value: myProjectResponse.id
        });
    }

    public async submitFormAsync() {
        if (this.form.valid) {
            await this.allotmentEventService.createAsync(this.form.value['eventName'],
                this.form.value['tokenAddress'],
                this.form.value['tokenDecimals'],
                this.form.value['ticker'],
                this.form.value['project'],
                this.form.value['finishDate']);
            this.dialogRef.close(true);
        }
    }

    public checkToken() {
    }
}
