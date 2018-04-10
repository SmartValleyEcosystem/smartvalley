import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-expert-selector',
  templateUrl: './expert-selector.component.html',
  styleUrls: ['./expert-selector.component.scss']
})
export class ExpertSelectorComponent implements OnInit {

  public maximumExperts = 6;
  public experts: string[] = [];
  public selectedClass = 'expert__selected';
  public selectedExpertsCount: number;

  @Output() selectedExpertsCountEmit: EventEmitter<{ [id: string]: number }> = new EventEmitter<{ [id: string]: number }>();
  @Input() expertPanelId: number;
  @Input() cost = 0;
  @Input() initialExpertsAmount: number;

  constructor() {
  }

  ngOnInit() {
    this.selectedExpertsCount = this.initialExpertsAmount;
    for (let i = 0; i < this.maximumExperts; i++) {
      this.experts.push('');
    }

    for (let i = 0; i < this.initialExpertsAmount; i++) {
      this.experts[i] = this.selectedClass;
    }
  }

  public addExpert() {
    if (this.selectedExpertsCount < this.maximumExperts) {
      this.experts[this.selectedExpertsCount] = this.selectedClass;
      this.selectedExpertsCount++;
    }
    this.selectedExpertsCountEmit.emit({[this.expertPanelId]: this.selectedExpertsCount});
  }

  public removeExpert() {
    if (this.selectedExpertsCount) {
      this.experts[this.selectedExpertsCount - 1] = '';
      this.selectedExpertsCount--;
    }
    this.selectedExpertsCountEmit.emit({[this.expertPanelId]: this.selectedExpertsCount});
  }
}
