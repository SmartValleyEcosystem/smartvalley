import {Injectable} from '@angular/core';
import {Application} from '../services/application';
import {TeamMember} from './team-member';
import {EnumTeamMemberType} from './enumTeamMemberType';

@Injectable()
export class ApplicationService {
  public applications: Array<Application>;

  constructor() {
    this.initTestData();
  }

  getAll(): Array<Application> {
    return this.applications;
  }

  getById(id: string): Application {
    return this.applications.filter(x => x.projectId === id)[0];
  }

  // тестовые данные
  initTestData() {
    this.applications = [];
    this.applications.push(<Application>{
      projectId: '0',
      name: 'Comp1',
      projectArea: 'Area1',
      country: 'Russia',
      whitePaperLink: 'http',
      teamMembers: [
        <TeamMember>{ fullName: 'Vasya Pupkin', memberType: EnumTeamMemberType.CEO, facebookLink: 'httpFB', linkedInLink: 'link1'},
        <TeamMember>{ fullName: 'Ivan Ivanov', memberType: EnumTeamMemberType.PR, facebookLink: 'httpFB1', linkedInLink: 'link13'},
        <TeamMember>{ fullName: 'Sidor Sidorov', memberType: EnumTeamMemberType.CTO, facebookLink: 'httpFB2', linkedInLink: 'link14'}
        ],
      projectStatus: 'Okay',
      solutionDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      probablyDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'
    });

    this.applications.push(<Application>{
      projectId: '1',
      name: 'Comp2',
      projectArea: 'Area2',
      country: 'USA',
      projectStatus: 'Okay',
      whitePaperLink: 'http',
      solutionDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      probablyDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'
    });
    this.applications.push(<Application>{
      projectId: '2',
      name: 'Comp3',
      projectArea: 'Area3',
      country: 'China',
      projectStatus: 'Okay',
      whitePaperLink: 'http',
      solutionDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      probablyDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'
    });
    this.applications.push(<Application>{
      projectId: '3',
      name: 'Comp4',
      projectArea: 'Area4',
      country: 'Japan',
      projectStatus: 'Okay',
      whitePaperLink: 'http',
      solutionDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      probablyDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'
    });
    this.applications.push(<Application>{
      projectId: '4',
      name: 'Comp5',
      projectArea: 'Area5',
      country: 'Italy',
      projectStatus: 'Okay',
      whitePaperLink: 'http',
      solutionDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.',
      probablyDescription: 'In literary theory, a text is any object that can be "read", whether this object is a work of literature, a street sign, an arrangement of buildings on a city block, or styles of clothing.'
    });
  }
}
