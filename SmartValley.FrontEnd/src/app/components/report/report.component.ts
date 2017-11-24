import {Component, OnInit} from '@angular/core';
import {Project} from '../../services/project';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';
import {ProjectService} from '../../services/project-service';
import {Application} from '../../services/application';
import {ApplicationService} from '../../services/application-service';
import {ActivatedRoute} from '@angular/router';

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
              private applicationService: ApplicationService,
              private route: ActivatedRoute) {
    const id = this.route.snapshot.paramMap.get('id');
    this.project = projectService.getById(parseInt(id, 0));
    this.application = applicationService.getById(id);
  }

  colorOfProjectRate(rate: number): string {
    if (rate == null) {
      return '';
    }
    if (rate > 80) {
      return 'high_rate';
    }
    if (rate > 45) {
      return 'medium_rate';
    }
    if (rate >= 0) {
      return 'low_rate';
    }
    return 'progress_rate';
  }

}
