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
    const idParam = this.route.snapshot.paramMap.get('id');
    const id = parseInt(idParam, 0);
    this.project = projectService.getById(id);
    this.loadApplication(id);
  }

  ngOnInit() {
  }

  private async loadApplication(id: number) {
    this.application = await this.applicationApiClient.getByProjectIdAsync(id);
  }
}
