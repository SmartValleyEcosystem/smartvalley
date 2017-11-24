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

  // тестовые данные
  initTestData() {
    this.questions = [];
    this.questions.push(<Question>{
      name: 'Team Completeness',
      description: 'somedesc1',
      expertType: EnumExpertType.HR,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Team Experience',
      description: 'somedesc2',
      score: 5,
      expertType: EnumExpertType.Technical_expert,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Attracted Investments',
      description: 'somedesc3',
      score: 5,
      expertType: EnumExpertType.Lawyer,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc4',
      score: 5,
      expertType: EnumExpertType.Lawyer,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Team Completeness',
      description: 'somedesc5',
      score: 5,
      expertType: EnumExpertType.Analyst,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc6',
      score: 5,
      expertType: EnumExpertType.Analyst,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc7',
      score: 5,
      expertType: EnumExpertType.Analyst,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
    this.questions.push(<Question>{
      name: 'Attracted Investments',
      description: 'somedesc8',
      score: 5,
      expertType: EnumExpertType.Analyst,
      estimates: [
        <Estimate>{score: '5', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '4', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
        <Estimate>{score: '6', comments: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'},
      ]
    });
  }
}
