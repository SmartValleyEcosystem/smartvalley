import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {AreaType} from '../../api/scoring/area-type.enum';
import {ProjectArea} from './project-area';
import {Stage} from './stage';
import {SocialMedia} from './social-media';
import {ScoringProjectStatus} from '../scoring-project-status.enum';
import {ProjectAreaTypeEnum} from './project-area-type.enum';
import {SocialMediaTypeEnum} from './social-media-type.enum';
import {QuestionService} from '../questions/question-service';
import {StageTypeEnum} from './stage-type.enum';

@Injectable()
export class ProjectService {
  constructor(private questionService: QuestionService) {
  }

  public getProjectAreas(): Array<ProjectArea> {
    return <Array<ProjectArea>>[
      {name: 'All areas', projectAreaType: null},
      {name: 'Art', projectAreaType: ProjectAreaTypeEnum.Art},
      {name: 'Artificial intelligence', projectAreaType: ProjectAreaTypeEnum.ArtificialIntelligence},
      {name: 'Banking', projectAreaType: ProjectAreaTypeEnum.Banking},
      {name: 'BigData', projectAreaType: ProjectAreaTypeEnum.BigData},
      {name: 'Business services', projectAreaType: ProjectAreaTypeEnum.BusinessServices},
      {name: 'Casino and gambling', projectAreaType: ProjectAreaTypeEnum.CasinoAndGambling},
      {name: 'Charity', projectAreaType: ProjectAreaTypeEnum.Charity},
      {name: 'Communication', projectAreaType: ProjectAreaTypeEnum.Communication},
      {name: 'Cryptocurrency', projectAreaType: ProjectAreaTypeEnum.Cryptocurrency},
      {name: 'Education', projectAreaType: ProjectAreaTypeEnum.Education},
      {name: 'Electronics', projectAreaType: ProjectAreaTypeEnum.Electronics},
      {name: 'Energy', projectAreaType: ProjectAreaTypeEnum.Energy},
      {name: 'Entertainment', projectAreaType: ProjectAreaTypeEnum.Entertainment},
      {name: 'Health', projectAreaType: ProjectAreaTypeEnum.Health},
      {name: 'Infrastructure', projectAreaType: ProjectAreaTypeEnum.Infrastructure},
      {name: 'Internet', projectAreaType: ProjectAreaTypeEnum.Internet},
      {name: 'Investment', projectAreaType: ProjectAreaTypeEnum.Investment},
      {name: 'Legal', projectAreaType: ProjectAreaTypeEnum.Legal},
      {name: 'Manufacturing', projectAreaType: ProjectAreaTypeEnum.Manufacturing},
      {name: 'Media', projectAreaType: ProjectAreaTypeEnum.Media},
      {name: 'Other', projectAreaType: ProjectAreaTypeEnum.Other},
      {name: 'Platform', projectAreaType: ProjectAreaTypeEnum.Platform},
      {name: 'Real estate', projectAreaType: ProjectAreaTypeEnum.RealEstate},
      {name: 'Retail', projectAreaType: ProjectAreaTypeEnum.Retail},
      {name: 'Smart contract', projectAreaType: ProjectAreaTypeEnum.SmartContract},
      {name: 'Software', projectAreaType: ProjectAreaTypeEnum.Software},
      {name: 'Sports', projectAreaType: ProjectAreaTypeEnum.Sports},
      {name: 'Tourism', projectAreaType: ProjectAreaTypeEnum.Tourism},
      {name: 'Virtual reality', projectAreaType: ProjectAreaTypeEnum.VirtualReality}
    ];
  }

  public getStages(): Array<Stage> {
    return <Array<Stage>>[
      {name: 'All stages', stageType: null},
      {name: 'Pre sale', stageType: StageTypeEnum.PreSale}
    ];
  }

  public getSocialMedias(): Array<SocialMedia> {
    return <Array<SocialMedia>>[
      {name: 'All socials', socialMediaType: null},
      {name: 'Bitcoin Talk', socialMediaType: SocialMediaTypeEnum.BitcoinTalk},
      {name: 'Facebook', socialMediaType: SocialMediaTypeEnum.Facebook},
      {name: 'Github', socialMediaType: SocialMediaTypeEnum.Github},
      {name: 'LinkedIn', socialMediaType: SocialMediaTypeEnum.LinkedIn},
      {name: 'Medium', socialMediaType: SocialMediaTypeEnum.Medium},
      {name: 'Reddit', socialMediaType: SocialMediaTypeEnum.Reddit},
      {name: 'Telegram', socialMediaType: SocialMediaTypeEnum.Telegram},
      {name: 'Twitter', socialMediaType: SocialMediaTypeEnum.Twitter}
    ];
  }

  public colorOfAreaScore(score: number, areaType: AreaType): string {
    const maxScore = this.questionService.getMaxScoreForArea(areaType);
    const minScore = this.questionService.getMinScoreForArea(areaType);

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
