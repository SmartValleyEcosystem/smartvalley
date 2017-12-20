import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';

@Component({
  selector: 'app-link-modal',
  templateUrl: './link-modal.component.html',
  styleUrls: ['./link-modal.component.css']
})
export class LinkModalComponent {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  }

}
