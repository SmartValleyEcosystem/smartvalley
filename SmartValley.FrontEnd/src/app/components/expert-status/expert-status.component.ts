import {Component, OnInit} from '@angular/core';
import {Paths} from '../../paths';
import {Router, ActivatedRoute} from '@angular/router';
import {ExpertApplicationStatus} from '../../services/expert/expert-application-status.enum';
import {ExpertApiClient} from '../../api/expert/expert-api-client';
import {UserContext} from '../../services/authentication/user-context';

@Component({
  selector: 'app-expert-status',
  templateUrl: './expert-status.component.html',
  styleUrls: ['./expert-status.component.css']
})
export class ExpertStatusComponent implements OnInit {

  public ExpertApplicationStatus = ExpertApplicationStatus;

  public applicationStatus: ExpertApplicationStatus;

  constructor(private route: ActivatedRoute,
              private expertApiClient: ExpertApiClient,
              private userContext: UserContext,
              private router: Router) {
  }

  async ngOnInit() {
    const address = this.userContext.getCurrentUser().account;
    const expertStatusResponse = await this.expertApiClient.getExpertStatusAsync(address);
    this.applicationStatus = expertStatusResponse.status;
  }

  apply(): void {
    this.router.navigate([Paths.RegisterExpert]);
  }
}
