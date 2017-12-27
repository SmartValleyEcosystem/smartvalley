import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {AlertModalData} from './alert-modal-data';

@Component({
  selector: 'app-alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.css']
})
export class AlertModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: AlertModalData) {
  }

  ngOnInit() {
  }
}
