import {Injectable} from '@angular/core';
import {Scoring} from '../services/scoring';

@Injectable()
export class ScoringService {
  public scorings: Array<Scoring>;

  constructor() {
    this.initTestData();
  }

  getAll(): Array<Scoring> {
    return this.scorings;
  }

  getById(id: number): Scoring {
    return this.scorings.filter(x => x.projectId === id)[0];
  }

  // тестовые данные
  initTestData() {
    this.scorings = [];
    this.scorings.push(<Scoring>{
      projectId: 0,
      projectName: 'Rega Risk Sharing',
      projectArea: 'Crowdsurance',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectId: 1,
      projectName: 'BitClave Active Search Ecosystem',
      projectArea: 'Rotetechnology',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectId: 2,
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectId: 3,
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectId: 4,
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });
  }
}
