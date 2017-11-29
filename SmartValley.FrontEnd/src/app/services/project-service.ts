import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';

@Injectable()
export class ProjectService {

  public colorOfEstimateScore(rate: number): string {
    if (isNullOrUndefined(rate)) {
      return 'progress_rate';
    }
    if (rate > 4) {
      return 'high_rate';
    }
    if (rate > 2) {
      return 'medium_rate';
    }
    return 'low_rate';
  }

  public colorOfProjectRate(rate: number): string {
    if (rate == null) {
      return '';
    }
    if (rate > 80) {
      return 'high_rate';
    }
    if (rate > 45) {
      return 'medium_rate';
    }
    return 'low_rate';
  }
}

