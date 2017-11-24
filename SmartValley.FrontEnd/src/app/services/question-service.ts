import {Injectable} from '@angular/core';
import {Question} from '../services/question';

;
import {Estimate} from '../services/estimate';
import {EnumExpertType} from '../services/enumExpertType';
import {EnumTeamMemberType} from './enumTeamMemberType';

@Injectable()
export class QuestionService {
  private questions: { [expertType: number]: Array<Question>; } = {};

  constructor() {
    this.initializeQuestionsCollection();
  }

  public getByExpertType(expertType: EnumExpertType): Array<Question> {
    return this.questions[expertType];
  }
  private initializeQuestionsCollection() {
    this.questions[EnumExpertType.HR] = [
      <Question>{
        name: 'Team Completeness',
        description: 'If all major roles are closed - 6.\n' +
        'If any role is missing minus 1 for each, minus 2 for CEO.\n' +
        'If specialist doesn\'t have any experience, disregard him.',
        maxScore: 6,
        indexInCategory: 0,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Team Experience',
        description: 'For each role:\n' +
        'If experience less than 2 years - 1, more - 2\n' +
        'Don\'t have experience - 0',
        maxScore: 10,
        indexInCategory: 1,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Attracted Investments',
        description: 'If anyone in the team attracted investments before - 3.',
        maxScore: 3,
        indexInCategory: 2,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Scam',
        description: 'If anyone in the team seen in scam projects - minus 15.\n' +
        'If no one in the team seen in scam projects - 0.',
        maxScore: 0,
        indexInCategory: 3,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      }
    ];

    this.questions[EnumExpertType.Lawyer] = [
      <Question>{
        name: 'Incorporation Risk',
        description: 'If incorporation has minimal risk - 10.\n' +
        'If incorporation has medium risk - 6.\n' +
        'If incorporation has high risk - 2.\n' +
        'Project doesn\'t has incorporation - 0.',
        maxScore: 10,
        indexInCategory: 0,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Token Structure',
        description: 'Utility token - 15.\n' +
        'Security token, but company has SAC license - 5.\n' +
        'Security token = 0.',
        maxScore: 15,
        indexInCategory: 1,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      }
    ];

    this.questions[EnumExpertType.Analyst] = [
      <Question>{
        name: 'Involved Amount',
        description: 'If amount involved is commensurate with the tasks of the project and less than 15 mln USD - 8.\n' +
        'If amount involved is commensurate with the tasks of the project and more than 15 mln USD - 6.\n' +
        'If amount involved is disproportionate with the tasks of the project and less than 15 mln USD - 4.\n' +
        'If amount involved is disproportionate with the tasks of the project and more than 15 mln USD - 2.',
        maxScore: 8,
        indexInCategory: 0,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Financial Model',
        description: 'Efficient financial model, to make the scaled income grow not linear - 10.\n' +
        'Efficient financial model, to make the scaled income grow linear - 6.\n' +
        'No efficient finance model - 0.',
        maxScore: 10,
        indexInCategory: 1,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Idea',
        description: 'Project has economic advantages and doesn\'t have competitors - 10.\n' +
        'Project has economic advantages - 7.\n' +
        'Project doesn\'t have economic advantages, but has other advantages - 4.\n' +
        'Project doesn\'t have advantages - 0.',
        maxScore: 10,
        indexInCategory: 2,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
    ];

    this.questions[EnumExpertType.TechnicalExpert] = [
      <Question>{
        name: 'Blockchain Usage',
        description: 'Blockchain is required - 5.\n' +
        'Blockchain solved minor problems - 2.\n' +
        'Blockchain is not needed - 0.',
        maxScore: 5,
        indexInCategory: 0,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Blockchain Opportunity',
        description: 'Can be realised on existing blockchain protocols - 6.\n' +
        'Can\'t be realised on existing blockchain protocols - 0.\n',
        maxScore: 6,
        indexInCategory: 1,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Chosen Blockchain',
        description: 'Chosen blockchain protocol provides a way to realize all functionality - 5.\n' +
        'Chosen blockchain protocol provides a way to realize only main functionality - 3.\n' +
        'Chosen blockchain protocol doesn\'t provide a way to realize main functionality - 0.',
        maxScore: 5,
        indexInCategory: 2,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
      <Question>{
        name: 'Prototype/MVP',
        description: 'Prototype solved state problem and used blockchain - 10.\n' +
        'Prototype solved state problem but didn\'t use blockchain - 6.\n' +
        'Prototype didn\'t solve state problem and used blockchain - 2.\n' +
        'Prototype didn\'t solve state problem and didn\'t use blockchain - 0.',
        maxScore: 10,
        indexInCategory: 3,
        estimates: [
          <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
          <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        ]
      },
    ];
  }
}
