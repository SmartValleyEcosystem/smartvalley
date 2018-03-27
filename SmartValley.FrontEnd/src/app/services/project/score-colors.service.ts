import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {AreaType} from '../../api/scoring/area-type.enum';
import {ScoringProjectStatus} from '../scoring-project-status.enum';
import {ScoringCriterionService} from '../criteria/scoring-criterion.service';
import {Score} from '../score.enum';
import {AreaService} from '../expert/area.service';

@Injectable()
export class ScoreColorsService {
  constructor(private scoringCriterionService: ScoringCriterionService,
              private areaService: AreaService) {
  }

  public getAreaScoreColor(score: number, areaType: AreaType): string {
    const maxScore = this.areaService.getMaxScore(areaType);
    const percent = (score / maxScore) * 100;
    return this.getColorByPercent(percent);
  }

  public getEstimateScoreColor(score: Score): string {
    switch (score) {
      case Score.High:
        return 'high_rate';
      case Score.Low:
        return 'low_rate';
      case Score.Medium:
        return 'medium_rate';
    }
  }

  public getProjectStatusColor(status: ScoringProjectStatus): string {
    if (isNullOrUndefined(status)) {
      return '';
    }
    return this.getColorByStatus(status);
  }

  public getProjectScoreColor(score: number): string {
    if (score == null) {
      return '';
    }
    return this.getColorByPercent(score);
  }

  private getColorByStatus(status: ScoringProjectStatus): string {
    switch (status) {
      case ScoringProjectStatus.InProgress:
        return 'high_rate';
      case ScoringProjectStatus.AcceptedAndDoNotEstimate:
        return 'low_rate';
      case ScoringProjectStatus.Rejected:
        return 'medium_rate';
    }
  }

  private getColorByPercent(percent: number): string {
    if (percent > 67) {
      return 'high_rate';
    }
    if (percent > 33) {
      return 'medium_rate';
    }
    return 'low_rate';
  }
}
