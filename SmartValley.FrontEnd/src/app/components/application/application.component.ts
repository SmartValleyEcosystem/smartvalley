import { Component } from '@angular/core';

@Component({
  selector: 'app-application',
  templateUrl: './application.component.html',
  styleUrls: ['./application.component.css']
})
export class ApplicationComponent {

  name = '';

  projectArea = '';

  probDesc = '';

  solDesc = '';

  projStat = '';

  wpLink = '';

  blockChainType = '';

  country = '';

  mvpLink = '';

  softCap = '';

  hardCap = '';

  attractInv = '';

  CEO = new TeamMember();

  CFO = new TeamMember();

  CMO = new TeamMember();

  CTO = new TeamMember();

  PR = new TeamMember();


  diagnostic() {
    const json = JSON.stringify(this);
    console.log(json);
  }
}

export class TeamMember {

  fullName = '';

  fbLink = '';

  linkedInLink = '';
}


