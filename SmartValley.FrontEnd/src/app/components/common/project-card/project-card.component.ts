import {Component, Input, OnInit} from '@angular/core';
import {Project} from '../../../services/project';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {ProjectService} from '../../../services/project-service';
import {BlockiesService} from '../../../services/blockies-service';
import {Constants} from '../../../constants';
import {ProjectCardType} from '../../../services/project-card-type';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent implements OnInit {
  public ProjectCardType = ProjectCardType;
  @Input() public project: Project;
  @Input() public projectCardType: ProjectCardType;
  projectService: ProjectService;
  projectImageUrl: any;

  constructor(private router: Router,
              private blockiesService: BlockiesService,
              projectService: ProjectService) {
    this.projectService = projectService;
  }

  ngOnInit(): void {
    this.projectImageUrl = this.blockiesService.getImageForAddress(this.project.address ? this.project.address : this.project.author);
  }

  showProject(id: number) {
    this.router.navigate([Paths.Scoring + '/' + id], {queryParams: {expertiseArea: this.project.expertiseArea}});
  }

  voteForProject(id: number) {
    this.router.navigate([Paths.Vote + '/' + id]);
  }

  showReport(id: number) {
    this.router.navigate([Paths.Report + '/' + id], {queryParams: {tab: Constants.ReportFormTab}});
  }
}
