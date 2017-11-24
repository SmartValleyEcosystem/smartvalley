import {Component, OnInit} from '@angular/core';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ActivatedRoute} from '@angular/router';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  report: ProjectDetailsResponse;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute) {
    this.loadData();
  }

  private async loadData() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.report = await this.projectApiClient.getDetailsByIdAsync(id);
  }
}
