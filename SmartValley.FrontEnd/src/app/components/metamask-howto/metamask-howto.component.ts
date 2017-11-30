import {Component, OnInit} from '@angular/core';
import {Web3Service} from '../../services/web3-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';
import {Ng2DeviceService} from 'ng2-device-detector';


@Component({
  selector: 'app-metamask-howto',
  templateUrl: './metamask-howto.component.html',
  styleUrls: ['./metamask-howto.component.css']
})
export class MetamaskHowtoComponent {

  constructor(private deviceService: Ng2DeviceService) {
    this.browser = deviceService.browser;
  }

  public browser: string;
}
