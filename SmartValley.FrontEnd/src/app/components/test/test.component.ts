import {Component, OnInit} from '@angular/core';
import {TestService} from "../../backend/api/test.service";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  constructor(private testService: TestService) {
  }

  ngOnInit() {
  }

  gotoApiAnonymous() {
    this.testService.apiTestGetCustomGet().subscribe(
      data => {
        alert(data.value);
      },
      err => {
        alert(err);
      });
  }

  gotoApi() {
    this.testService.apiTestGet().subscribe(
      data => {
        alert(data);
      },
      err => {
        alert(err);
      });
  }
}
