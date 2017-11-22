import {Injectable} from '@angular/core';
import {Project} from './project';

@Injectable()
export class ProjectService {
  public projects: Array<Project>;

  constructor() {
    this.initTestData();
  }

  getAll(): Array<Project> {
    return this.projects;
  }

  // тестовые данные
  initTestData() {
    this.projects = [];
    this.projects.push(<Project>{
      name: 'Team Completeness',
      imageUrl: 'https://png.icons8.com/?id=50284&size=280',
      country: 'Russia',
      area: 'Bio',
      description: 'desc1',
      score: 'in progress'
    });
    this.projects.push(<Project>{
      name: 'Team Completeness',
      imageUrl: 'https://png.icons8.com/?id=50284&size=280',
      country: 'Russia',
      area: 'Bio',
      description: 'desc1',
      score: 'in progress'
    });
    this.projects.push(<Project>{
      name: 'Team Completeness',
      imageUrl: 'https://png.icons8.com/?id=50284&size=280',
      country: 'Russia',
      area: 'Bio',
      description: 'desc1',
      score: 'in progress'
    });
    this.projects.push(<Project>{
      name: 'Team Completeness',
      imageUrl: 'https://png.icons8.com/?id=50284&size=280',
      country: 'Russia',
      area: 'Bio',
      description: 'desc1',
      score: 'in progress'
    });
  }
}
