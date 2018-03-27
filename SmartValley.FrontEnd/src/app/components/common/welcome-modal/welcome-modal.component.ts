import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA} from '@angular/material';
import {WelcomeModalData} from './welcome-modal-data';

@Component({
  selector: 'app-welcome-modal',
  templateUrl: './welcome-modal.component.html',
  styleUrls: ['./welcome-modal.component.css']
})
export class WelcomeModalComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: WelcomeModalData) { }

  ngOnInit() {
  }
}
