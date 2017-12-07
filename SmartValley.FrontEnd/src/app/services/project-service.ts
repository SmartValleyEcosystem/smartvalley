import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {QuestionService} from './question-service';
import {ScoringCategory} from '../api/scoring/scoring-category.enum';

@Injectable()
export class ProjectService {
  constructor(private questionService: QuestionService) {
  }

  public colorOfHrScore(score: number) {
    return this.colorOfCategoryScore(score, ScoringCategory.HR);
  }

  public colorOfTechnicalScore(score: number) {
    return this.colorOfCategoryScore(score, ScoringCategory.TechnicalExpert);
  }

  public colorOfLawyerScore(score: number) {
    return this.colorOfCategoryScore(score, ScoringCategory.Lawyer);
  }

  public colorOfAnalystScore(score: number) {
    return this.colorOfCategoryScore(score, ScoringCategory.Analyst);
  }

  public colorOfCategoryScore(score: number, category: ScoringCategory) {
    const maxScore = this.questionService.getMaxScoreForCategory(category);
    const minScore = this.questionService.getMinScoreForCategory(category);

    return this.getColorInRange(score, minScore, maxScore);
  }

  public colorOfEstimateScore(score: number, minScore: number, maxScore: number): string {
    if (isNullOrUndefined(score)) {
      return 'progress_rate';
    }

    return this.getColorInRange(score, minScore, maxScore);
  }

  public colorOfProjectRate(score: number): string {
    if (score == null) {
      return '';
    }
    return this.getColorByPercent(score);
  }

  private getColorInRange(score: number, minScore: number, maxScore: number): string {
    const percent = ((score - minScore) / (maxScore - minScore)) * 100;
    return this.getColorByPercent(percent);
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
