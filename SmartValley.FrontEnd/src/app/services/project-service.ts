import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {QuestionService} from './questions/question-service';
import {ExpertiseArea} from '../api/scoring/expertise-area.enum';
import {ScoringProjectStatus} from './scoring-project-status.enum';

@Injectable()
export class ProjectService {
  constructor(private questionService: QuestionService) {
  }

  public colorOfHrScore(score: number): string {
    return this.colorOfExpertiseAreaScore(score, ExpertiseArea.HR);
  }

  public colorOfTechnicalScore(score: number): string {
    return this.colorOfExpertiseAreaScore(score, ExpertiseArea.TechnicalExpert);
  }

  public colorOfLawyerScore(score: number): string {
    return this.colorOfExpertiseAreaScore(score, ExpertiseArea.Lawyer);
  }

  public colorOfAnalystScore(score: number): string {
    return this.colorOfExpertiseAreaScore(score, ExpertiseArea.Analyst);
  }

  public colorOfExpertiseAreaScore(score: number, expertiseArea: ExpertiseArea): string {
    const maxScore = this.questionService.getMaxScoreForExpertiseArea(expertiseArea);
    const minScore = this.questionService.getMinScoreForExpertiseArea(expertiseArea);

    return this.getColorInRange(score, minScore, maxScore);
  }

  public colorOfEstimateScore(score: number, minScore: number, maxScore: number): string {
    if (isNullOrUndefined(score)) {
      return 'progress_rate';
    }

    return this.getColorInRange(score, minScore, maxScore);
  }

  public colorOfProjectStatus(status: ScoringProjectStatus): string {
    if (isNullOrUndefined(status)) {
      return '';
    }
    return this.getColorByStatus(status);
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
