import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ProjectService} from '../../services/project-service';
import {Application} from '../../services/application';
import {ActivatedRoute} from '@angular/router';
import {ApplicationApiClient} from '../../api/application/application-api.client';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {

  project: Project;
  application: Application;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private projectService: ProjectService,
              private applicationApiClient: ApplicationApiClient,
              private route: ActivatedRoute) {
    this.loadApplication();
  }

  ngOnInit() {
  }

  private async loadApplication() {
    const id = +this.route.snapshot.paramMap.get('id');
    this.project = this.projectService.getById(id);
    this.application = await this.applicationApiClient.getByProjectIdAsync(id);
  }
}
