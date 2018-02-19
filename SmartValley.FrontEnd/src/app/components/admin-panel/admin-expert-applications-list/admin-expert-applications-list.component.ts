import {Component, OnInit} from '@angular/core';
import {ExpertApiClient} from '../../../api/expert/expert-api-client';
import * as moment from 'moment';
import {AdminExpertApplicationItem} from './admin-expert-application-item';

@Component({
  selector: 'app-admin-expert-applications-list',
  templateUrl: './admin-expert-applications-list.component.html',
  styleUrls: ['./admin-expert-applications-list.component.css']
})
export class AdminExpertApplicationsListComponent implements OnInit {

  constructor(private expertApiClient: ExpertApiClient) {
  }

  public applications: AdminExpertApplicationItem[];

  async ngOnInit() {
    this.applications = [];
    const response = await this.expertApiClient.getPendingApplicationsAsync();
    for (const application of response.items) {
      const applicationItem = <AdminExpertApplicationItem>{
        id: application.id,
        firstName: application.firstName,
        lastName: application.lastName,
        applyDate: moment(application.applyDate).toDate()
      };
      this.applications.push(applicationItem);
    }
  }

  public viewForm() {

  }
}
