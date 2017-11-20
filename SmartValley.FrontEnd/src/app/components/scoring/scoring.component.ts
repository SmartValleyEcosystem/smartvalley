import {Component} from '@angular/core';
import {Scoring} from '../../services/scoring';

@Component({
  selector: 'app-scoring',
  templateUrl: './scoring.component.html',
  styleUrls: ['./scoring.component.css']
})
export class ScoringComponent {

  public scorings: Array<Scoring>;

  constructor() {
    this.initTestData();
  }

  // тестовые данные
  initTestData() {
    this.scorings = [];
    this.scorings.push(<Scoring>{
      projectName: 'Rega Risk Sharing',
      projectArea: 'Crowdsurance',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectName: 'BitClave Active Search Ecosystem',
      projectArea: 'Rotetechnology',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
      projectName: 'B2Broker',
      projectArea: 'Brokering',
      projectCountry: 'Russia',
      scoringRating: 'in progress',
      projectDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      expertType: 'HR',
      projectImgUrl: 'https://png.icons8.com/?id=50284&size=280'
    });

    this.scorings.push(<Scoring>{
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
