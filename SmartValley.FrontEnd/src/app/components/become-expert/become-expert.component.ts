import {Component} from '@angular/core';
import {Paths} from '../../paths';
import {Router} from '@angular/router';

@Component({
  selector: 'app-become-expert',
  templateUrl: './become-expert.component.html',
  styleUrls: ['./become-expert.component.css']
})
export class BecomeExpertComponent {

  constructor(private router: Router) {
  }

  apply(): void {
    this.router.navigate([Paths.RegisterExpert]);
  }
}
