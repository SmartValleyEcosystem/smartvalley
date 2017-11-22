import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {Application} from '../../services/application';
import {ApplicationService} from '../../services/application-service';
import {EnumTeamMemberType} from '../../services/enumTeamMemberType';

@Component({
  selector: 'app-estimate',
  templateUrl: './estimate.component.html',
  styleUrls: ['./estimate.component.css']
})
export class EstimateComponent implements OnInit {

  public application: Application;
  hidden: boolean;
  EnumTeamMemberType: typeof EnumTeamMemberType = EnumTeamMemberType;

  constructor(private route: ActivatedRoute,
              private service: ApplicationService) {
  }

  changeHidden() {
    this.hidden = true;
  }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    this.application = this.service.getById(id);
    console.log(this.application);
  }
}
