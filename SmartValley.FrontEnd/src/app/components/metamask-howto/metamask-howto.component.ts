import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Ng2DeviceService} from 'ng2-device-detector';


@Component({
  selector: 'app-metamask-howto',
  templateUrl: './metamask-howto.component.html',
  styleUrls: ['./metamask-howto.component.css']
})
export class MetamaskHowtoComponent {

  constructor(private router: Router,
              private deviceService: Ng2DeviceService) {
    this.browser = deviceService.browser;
  }

  public browser: string;

  async navigateToMainPage() {
    window.location.assign(Paths.Root);
  }
}
