import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ProjectService} from '../../services/project-service';
import {Application} from '../../services/application';
import {ActivatedRoute} from '@angular/router';
import {ApplicationApiClient} from '../../api/application/application-api.client';
import {ProjectApiClient} from '../../api/project/project-api-client';
import {ProjectDetailsResponse} from '../../api/project/project-details-response';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  report: ProjectDetailsResponse;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private projectApiClient: ProjectApiClient,
              private route: ActivatedRoute) {
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = parseInt(idParam, 0);

    this.loadData(id);
  }

  ngOnInit() {
  }

  private async loadData(id: number) {
    this.report = await this.projectApiClient.getDetailsByIdAsync(id);
  }
}
