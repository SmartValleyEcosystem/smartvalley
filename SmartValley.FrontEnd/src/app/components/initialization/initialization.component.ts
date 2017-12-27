import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterStateSnapshot} from '@angular/router';
import {InitializationService} from '../../services/initialization/initialization.service';

@Component({
  selector: 'app-initialization',
  templateUrl: './initialization.component.html',
  styleUrls: ['./initialization.component.css']
})
export class InitializationComponent implements OnInit {

  private returnUrl: string;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private initializationService: InitializationService) {
  }

  async ngOnInit() {
    this.route.queryParams
      .subscribe(params => {
        this.returnUrl = params.returnUrl || '/';
      });

    await this.initializationService.initializeAppAsync();

    await this.router.navigateByUrl(this.returnUrl);
  }
}
