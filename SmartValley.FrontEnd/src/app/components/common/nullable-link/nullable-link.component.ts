import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-nullable-link',
  templateUrl: './nullable-link.component.html',
  styleUrls: ['./nullable-link.component.css']
})
export class NullableLinkComponent {
  @Input() public href: string;

  constructor() {
  }
}
