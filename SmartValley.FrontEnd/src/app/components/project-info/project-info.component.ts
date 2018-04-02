import {Component, Input, OnInit} from '@angular/core';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.css']
})
export class ProjectInfoComponent implements OnInit {

  @Input() project: ProjectSummaryResponse;

  constructor(private dictionariesService: DictionariesService) { }

  public async ngOnInit() {
  }

  public get country(): string {
    return this.dictionariesService.countries.find(i => i.code === this.project.countryCode).name.toString();
  }

  public get stage(): string {
    return this.dictionariesService.stages.find(i => i.id === this.project.stageId).value.toString();
  }

  public get category(): string {
    return this.dictionariesService.categories.find(i => i.id === this.project.scoringStatus).value.toString();
  }

}
