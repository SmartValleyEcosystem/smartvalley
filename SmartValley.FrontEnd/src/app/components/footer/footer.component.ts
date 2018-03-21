import { Component, OnInit } from '@angular/core';
import {Paths} from '../../paths';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit {

  public projectsLink: string;

  constructor() { }

  ngOnInit() {
    this.projectsLink = Paths.ProjectList;
  }

}
