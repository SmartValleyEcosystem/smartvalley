import { Component, OnInit, Input } from '@angular/core';
import {Paths} from '../../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-allotment-event-card',
  templateUrl: './allotment-event-card.component.html',
  styleUrls: ['./allotment-event-card.component.scss']
})
export class AllotmentEventCardComponent implements OnInit {

  constructor(private router: Router) { }

  @Input() public event: AllotmentEventCardComponent;
  @Input() public finished = false;

  ngOnInit() {
  }

  public getProjectLink(id) {
      return decodeURIComponent(
          this.router.createUrlTree([Paths.Project + '/' + id]).toString()
      );
  }
}
