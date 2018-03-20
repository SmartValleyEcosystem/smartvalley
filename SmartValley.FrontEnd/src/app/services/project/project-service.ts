import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import {AreaType} from '../../api/scoring/area-type.enum';
import {ProjectСategory} from './project-category';
import {Stage} from './stage';
import {SocialMedia} from './social-media';
import {ScoringProjectStatus} from '../scoring-project-status.enum';
import {ProjectCategoryEnum} from './project-category.enum';
import {SocialMediaTypeEnum} from './social-media-type.enum';
import {QuestionService} from '../questions/question-service';
import {StageTypeEnum} from './stage-type.enum';

@Injectable()
export class ProjectService {
  constructor(private questionService: QuestionService) {
  }

  public getProjectCategories(): Array<ProjectСategory> {
    return <Array<ProjectСategory>>[
      {name: 'All areas', projectCategory: null},
      {name: 'Art', projectCategory: ProjectCategoryEnum.Art},
      {name: 'Artificial intelligence', projectCategory: ProjectCategoryEnum.ArtificialIntelligence},
      {name: 'Banking', projectCategory: ProjectCategoryEnum.Banking},
      {name: 'BigData', projectCategory: ProjectCategoryEnum.BigData},
      {name: 'Business services', projectCategory: ProjectCategoryEnum.BusinessServices},
      {name: 'Casino and gambling', projectCategory: ProjectCategoryEnum.CasinoAndGambling},
      {name: 'Charity', projectCategory: ProjectCategoryEnum.Charity},
      {name: 'Communication', projectCategory: ProjectCategoryEnum.Communication},
      {name: 'Cryptocurrency', projectCategory: ProjectCategoryEnum.Cryptocurrency},
      {name: 'Education', projectCategory: ProjectCategoryEnum.Education},
      {name: 'Electronics', projectCategory: ProjectCategoryEnum.Electronics},
      {name: 'Energy', projectCategory: ProjectCategoryEnum.Energy},
      {name: 'Entertainment', projectCategory: ProjectCategoryEnum.Entertainment},
      {name: 'Health', projectCategory: ProjectCategoryEnum.Health},
      {name: 'Infrastructure', projectCategory: ProjectCategoryEnum.Infrastructure},
      {name: 'Internet', projectCategory: ProjectCategoryEnum.Internet},
      {name: 'Investment', projectCategory: ProjectCategoryEnum.Investment},
      {name: 'Legal', projectCategory: ProjectCategoryEnum.Legal},
      {name: 'Manufacturing', projectCategory: ProjectCategoryEnum.Manufacturing},
      {name: 'Media', projectCategory: ProjectCategoryEnum.Media},
      {name: 'Other', projectCategory: ProjectCategoryEnum.Other},
      {name: 'Platform', projectCategory: ProjectCategoryEnum.Platform},
      {name: 'Real estate', projectCategory: ProjectCategoryEnum.RealEstate},
      {name: 'Retail', projectCategory: ProjectCategoryEnum.Retail},
      {name: 'Smart contract', projectCategory: ProjectCategoryEnum.SmartContract},
      {name: 'Software', projectCategory: ProjectCategoryEnum.Software},
      {name: 'Sports', projectCategory: ProjectCategoryEnum.Sports},
      {name: 'Tourism', projectCategory: ProjectCategoryEnum.Tourism},
      {name: 'Virtual reality', projectCategory: ProjectCategoryEnum.VirtualReality}
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
