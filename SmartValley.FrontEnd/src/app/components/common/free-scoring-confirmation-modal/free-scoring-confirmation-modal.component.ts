import {Component} from '@angular/core';

@Component({
  selector: 'app-free-scoring-confirmation-modal',
  templateUrl: './free-scoring-confirmation-modal.component.html',
  styleUrls: ['./free-scoring-confirmation-modal.component.css']
})
export class FreeScoringConfirmationModalComponent {
  public agreed = false;

  constructor() {
  }
}
