import {Component, OnInit} from '@angular/core';
import {Web3Service} from '../../services/web3-service';
import {Router} from '@angular/router';
import {Paths} from '../../paths';


@Component({
  selector: 'app-metamask-howto',
  templateUrl: './metamask-howto.component.html',
  styleUrls: ['./metamask-howto.component.css']
})
export class MetamaskHowtoComponent {

  constructor(private web3Service: Web3Service, private router: Router) {
  }
}
