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
    const percent = (score / maxScore) * 100;
    return this.getColorByPercent(percent);
  }

  public colorOfEstimateScore(score: number, maxScore: number): string {
    if (score <= 0) {
      return 'low_rate';
    }

    if (isNullOrUndefined(score)) {
      return 'progress_rate';
    }

    const percent = (score / maxScore) * 100;
    return this.getColorByPercent(percent);
  }

  public colorOfProjectRate(score: number): string {
    if (score == null) {
      return '';
    }
    return this.getColorByPercent(score);
  }

  private getColorByPercent(percent: number): string {
    if (percent > 80) {
      return 'high_rate';
    }
    if (percent > 45) {
      return 'medium_rate';
    }
    return 'low_rate';
  }
}
