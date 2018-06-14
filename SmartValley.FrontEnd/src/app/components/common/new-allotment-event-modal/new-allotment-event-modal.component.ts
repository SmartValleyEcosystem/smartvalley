import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from '@angular/material';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {SelectItem} from 'primeng/api';
import {ProjectApiClient} from '../../../api/project/project-api-client';
import {AllotmentEventService} from '../../../services/allotment-event/allotment-event.service';
import {ProjectQuery} from '../../../api/project/project-query';
import {ProjectsOrderBy} from '../../../api/application/projects-order-by.enum';
import {SortDirection} from '../../../api/sort-direction.enum';
import {Erc223ContractClient} from '../../../services/contract-clients/erc223-contract-client';

@Component({
  selector: 'app-new-allotment-event-modal',
  templateUrl: './new-allotment-event-modal.component.html',
  styleUrls: ['./new-allotment-event-modal.component.scss']
})
export class NewAllotmentEventModalComponent implements OnInit {
  public form: FormGroup;
  public allowedProjects: SelectItem[] = [];
  public isFormSubmited = false;

  constructor(private formBuilder: FormBuilder,
              private allotmentEventService: AllotmentEventService,
              private dialogRef: MatDialogRef<NewAllotmentEventModalComponent>,
              private projectApiClient: ProjectApiClient,
              private erc223ContractClient: Erc223ContractClient) {
  }

  async ngOnInit() {
    this.form = this.formBuilder.group({
      project: ['', [Validators.required]],
      eventName: ['', [Validators.required]],
      tokenAddress: ['', [Validators.required]],
      ticker: ['', [Validators.required]],
      tokenDecimals: ['', [Validators.required]],
      finishDate: [''],
    });

    const projectResponse = await this.projectApiClient.getAsync(<ProjectQuery>{
      offset: 0,
      count: 100,
      onlyScored: false,
      orderBy: ProjectsOrderBy.CreationDate,
      direction: SortDirection.Descending
    });

    const projects = projectResponse.items;
    for (const project of projects) {
      this.allowedProjects.push({
        label: project.name,
        value: project.id
      });
    }
  }

  public async submitFormAsync() {
    this.isFormSubmited = true;
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

  public async checkTokenAsync(event) {
    if (event.target.value.length === 42) {
      this.form.controls['tokenDecimals'].setValue(await this.erc223ContractClient.getDecimalsAsync(event.target.value));
      this.form.controls['ticker'].setValue(await this.erc223ContractClient.getSymbolAsync(event.target.value));
    }
  }
}
