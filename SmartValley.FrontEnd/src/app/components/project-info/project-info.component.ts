import {Component, Input, OnInit} from '@angular/core';
import {DictionariesService} from '../../services/common/dictionaries.service';
import {ProjectSummaryResponse} from '../../api/project/project-summary-response';
import {BlockiesService} from '../../services/blockies-service';

@Component({
  selector: 'app-project-info',
  templateUrl: './project-info.component.html',
  styleUrls: ['./project-info.component.scss']
})
export class ProjectInfoComponent implements OnInit {

  @Input() project: ProjectSummaryResponse;

  constructor(private dictionariesService: DictionariesService,
              private blockiesService: BlockiesService) {
  }

  public async ngOnInit() {
  }

  public get country(): string {
    return this.dictionariesService.countries.find(i => i.code === this.project.countryCode).name.toString();
  }

  public get stage(): string {
    return this.dictionariesService.stages.find(i => i.id === this.project.stageId).value.toString();
  }

  public get category(): string {
    return this.dictionariesService.categories.find(i => i.id === this.project.category).value.toString();
  }

  public get imageUrl(): string {
    return this.project.imageUrl || this.blockiesService.getImageForAddress(this.project.authorAddress);
  }
}
