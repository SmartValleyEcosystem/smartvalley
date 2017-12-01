import {Injectable} from '@angular/core';
import {Question} from '../services/question';

import {ScoringCategory} from '../api/scoring/scoring-category.enum';


@Injectable()
export class QuestionService {
  private questions: { [expertType: number]: Array<Question>; } = {};

  constructor() {
    this.initializeQuestionsCollection();
  }

  public getByCategory(category: ScoringCategory): Array<Question> {
    return this.questions[category];
  }

  public getMaxScoreForCategory(category: ScoringCategory): number {
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.maxScore).reduce((previous, current) => previous + current);
  }

  public getMinScoreForCategory(category: ScoringCategory): number {
    const categoryQuestions = this.questions[category];
    return categoryQuestions.map(q => q.minScore).reduce((previous, current) => previous + current);
  }

  private initializeQuestionsCollection() {
    this.questions[ScoringCategory.HR] = [
      <Question>{
        name: 'Team completeness',
        description: 'In case all major roles are filled in - <strong>6</strong> points\n' +
        'In case any position is missing, minus <strong>1</strong> point for each specialist, minus <strong>2</strong> points for CEO\n' +
        'In case specialist doesn\'t have any experience, disregard him\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>6</strong> points',
        maxScore: 6,
        minScore: 0,
        indexInCategory: 0,
        estimates: []
      },
      <Question>{
        name: 'Team experience',
        description: 'Team experience (for each position)\n' +
        'Specialist\'s experience:\n' +
        '<span class="sub-list">Less than 2 years - <strong>1</strong> point</span>\n' +
        '<span class="sub-list">More than 2 years - <strong>2</strong> points</span>\n' +
        '<span class="sub-list">No experience - <strong>0</strong> points</span>\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>10</strong> points\n',
        maxScore: 10,
        minScore: 0,
        indexInCategory: 1,
        estimates: []
      },
      <Question>{
        name: 'Attracted investments',
        description: 'In case anyone in the team attracted investments before - <strong>3</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>3</strong> points',
        maxScore: 3,
        minScore: 0,
        indexInCategory: 2,
        estimates: []
      },
      <Question>{
        name: 'Scam',
        description: 'In case anyone in the team was noticed in scam projects - minus <strong>15</strong> points\n' +
        'In case no one in the team was noticed in scam projects - <strong>0</strong> points\n' +
        '\nMinumum: <strong>-15</strong> points, Maximum: <strong>0</strong> points',
        maxScore: 0,
        minScore: -15,
        indexInCategory: 3,
        estimates: []
      }
    ];

    this.questions[ScoringCategory.Lawyer] = [
      <Question>{
        name: 'Incorporation risk',
        description: 'In case incorporation has minimal risk - <strong>10</strong> points\n' +
        'In case incorporation has medium risk - <strong>6</strong> points\n' +
        'In case incorporation has high risk - <strong>2</strong> points\n' +
        'Project has no incorporation - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>10</strong> points',
        maxScore: 10,
        minScore: 0,
        indexInCategory: 0,
        estimates: []
      },
      <Question>{
        name: 'Token structure',
        description: 'In case of Utility token - <strong>15</strong> points\n' +
        'In case of Security token, but company has SEC license - <strong>5</strong> points\n' +
        'In case of Security token - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>15</strong> points',
        maxScore: 15,
        minScore: 0,
        indexInCategory: 1,
        estimates: []
      }
    ];

    this.questions[ScoringCategory.Analyst] = [
      <Question>{
        name: 'Amount involved',
        description: 'In case amount involved is commensurate with the project\'s goals and is less than 15 mln. USD - <strong>8</strong> points\n' +
        'In case amount involved is commensurate with the project\'s goals and is more than 15 mln. USD - <strong>6</strong> points\n' +
        'In case amount involved is disproportionate with the project\'s goals and is less than 15 mln. USD - <strong>4</strong> points\n' +
        'In case amount involved is disproportionate with the project\'s goals and more than 15 mln. USD - <strong>2</strong> points\n' +
        '\nMinimum: <strong>2</strong> points, Maximum: <strong>8</strong> points',
        maxScore: 8,
        minScore: 0,
        indexInCategory: 0,
        estimates: []
      },
      <Question>{
        name: 'Financial model analysis',
        description: 'Efficient financial model with exponential scaled income growth - <strong>10</strong> points\n' +
        'Efficient financial model with linear scaled income growth - <strong>6</strong> points\n' +
        'No efficient finance model - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>10</strong> points',
        maxScore: 10,
        minScore: 0,
        indexInCategory: 1,
        estimates: []
      },
      <Question>{
        name: 'Idea and it\'s implementation analysis in terms of business',
        description: 'Project has economic advantages and doesn\'t have competitors - <strong>10</strong> points\n' +
        'Project has economic advantages - <strong>7</strong> points\n' +
        'Project has no economic advantages, but has other advantages - <strong>4</strong> points\n' +
        'Project has no advantages - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>10</strong> points',
        maxScore: 10,
        minScore: 0,
        indexInCategory: 2,
        estimates: []
      },
    ];

    this.questions[ScoringCategory.TechnicalExpert] = [
      <Question>{
        name: 'Blockchain use effectiveness',
        description: 'Blockchain is required - <strong>5</strong> points\n' +
        'Blockchain solves minor problems - <strong>2</strong> points\n' +
        'Blockchain is not required - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>5</strong> points',
        maxScore: 5,
        minScore: 0,
        indexInCategory: 0,
        estimates: []
      },
      <Question>{
        name: 'Implementation on existing blockchains',
        description: 'Can be realised on existing blockchain protocols - <strong>6</strong> points\n' +
        'Can\'t be realised on existing blockchain protocols - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum: <strong>6</strong> points',
        maxScore: 6,
        minScore: 0,
        indexInCategory: 1,
        estimates: []
      },
      <Question>{
        name: 'Blockchain selection',
        description: 'Correct blockchain selected, protocol provides a way to realize all functionality - <strong>5</strong> points\n' +
        'Correct blockchain selected, protocol provides a way to realize only main functionality - <strong>3</strong> points\n' +
        'Uncorrect blockchain selected - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum <strong>5</strong> points',
        maxScore: 5,
        minScore: 0,
        indexInCategory: 2,
        estimates: []
      },
      <Question>{
        name: 'Prototype/MVP analysis',
        description: 'Prototype solves stated problem by means of blockchain - <strong>10</strong> points\n' +
        'Prototype solves stated problem without blockchain use - <strong>6</strong> points\n' +
        'Prototype does not solve stated problem, however blockchain is used - <strong>2</strong> points\n' +
        'Prototype does not solve stated problem - <strong>0</strong> points\n' +
        '\nMinimum: <strong>0</strong> points, Maximum <strong>10</strong> points',
        maxScore: 10,
        minScore: 0,
        indexInCategory: 3,
        estimates: []
      },
    ];
  }
}
