import {Component, Input, OnInit} from '@angular/core';
import {Project} from '../../../services/project';
import {Router} from '@angular/router';
import {Paths} from '../../../paths';
import {ProjectService} from '../../../services/project-service';
import {BlockiesService} from '../../../services/blockies-service';

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.css']
})
export class ProjectCardComponent implements OnInit {
  @Input() public project: Project;
  @Input() public isScoring: boolean;
  projectService: ProjectService;
  projectImageUrl: any;

  constructor(private router: Router,
              private blockiesService: BlockiesService,
              projectService: ProjectService) {
    this.projectService = projectService;
  }

  ngOnInit(): void {
    this.projectImageUrl = this.blockiesService.getImageForAddress(this.project.address);
  }

  showProject(id: number) {
    this.router.navigate(
      [Paths.Scoring + '/' + id],
      {queryParams: {category: this.project.scoringCategory}});
  }

  showReport(id: number) {
    this.router.navigate([Paths.Report + '/' + id]);
  }
}
