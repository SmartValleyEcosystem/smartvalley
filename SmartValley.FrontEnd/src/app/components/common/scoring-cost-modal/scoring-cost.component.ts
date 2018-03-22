import {Component, OnInit} from '@angular/core';
import {AreaService} from '../../../services/expert/area.service';
import {Area} from '../../../services/expert/area';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ScoringService} from '../../../services/scoring/scoring.service';
import {MatDialogRef} from "@angular/material";

@Component({
  selector: 'app-scoring-cost',
  templateUrl: './scoring-cost.component.html',
  styleUrls: ['./scoring-cost.component.css']
})
export class ScoringCostComponent implements OnInit {

  public areas: Area[];
  public scoringCostForm: FormGroup;
  public isLoaded: boolean;

  constructor(private areaService: AreaService,
              private formBuilder: FormBuilder,
              private scoringService: ScoringService,
              private dialogRef: MatDialogRef<ScoringCostComponent>) {
    this.areas = this.areaService.areas;
  }

  async ngOnInit() {
    const scoringCosts = [];
    for (const item of this.areas) {
      const cost = await  this.scoringService.getScoringCostInAreaAsync(item.areaType);
      const group = this.formBuilder.group({
        areaType: item.areaType,
        name: item.name,
        cost: cost
      });
      scoringCosts.push(group);
    }

    this.scoringCostForm = this.formBuilder.group({
      areas: this.formBuilder.array(scoringCosts)
    });
    this.isLoaded = true;
  }

  public async submitAsync() {
    const areas = [];
    const costs = [];
    for (const area of this.scoringCostForm.value.areas) {
      areas.push(area.areaType);
      costs.push(area.cost);
    }
    await this.scoringService.setScoringCostAsync(areas, costs);
    this.dialogRef.close();
  }

}
