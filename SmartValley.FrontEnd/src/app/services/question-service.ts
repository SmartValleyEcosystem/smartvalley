import {Injectable} from '@angular/core';
import {Question} from '../services/question';
import {EnumExpertType} from '../services/enumExpertType';

@Injectable()
export class QuestionService {
  public questions: Array<Question>;

  constructor() {
    this.initTestData();
  }

  getByExpertType(expertType: EnumExpertType):  Array<Question> {
    return this.questions.filter(x => x.expertType === expertType);
  }

  // тестовые данные
  initTestData() {
    this.questions = [];
    this.questions.push(<Question>{
      name: 'Team Completeness',
      description: 'somedesc1',
      maxScore: 5,
      expertType: EnumExpertType.HR
    });
    this.questions.push(<Question>{
      name: 'Team Experience',
      description: 'somedesc2',
      maxScore: 5,
      expertType: EnumExpertType.Technical_expert
    });
    this.questions.push(<Question>{
      name: 'Attracted Investments',
      description: 'somedesc3',
      maxScore: 5,
      expertType: EnumExpertType.Lawyer
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc4',
      maxScore: 5,
      expertType: EnumExpertType.Lawyer
    });
    this.questions.push(<Question>{
      name: 'Team Completeness',
      description: 'somedesc5',
      maxScore: 5,
      expertType: EnumExpertType.Analyst
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc6',
      maxScore: 5,
      expertType: EnumExpertType.Analyst
    });
    this.questions.push(<Question>{
      name: 'Scam',
      description: 'somedesc7',
      maxScore: 5,
      expertType: EnumExpertType.Analyst
    });
    this.questions.push(<Question>{
      name: 'Attracted Investments',
      description: 'somedesc8',
      maxScore: 5,
      expertType: EnumExpertType.Analyst
    });
  }
}
