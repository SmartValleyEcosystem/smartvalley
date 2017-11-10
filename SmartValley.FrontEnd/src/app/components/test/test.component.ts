import {Component, NgZone, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {isNullOrUndefined} from 'util';
import {HttpClient, HttpHeaders} from '@angular/common/http';



@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent {

  private web3;


  constructor(private router: Router, private ngZone: NgZone, private http: HttpClient) {

    if ((window['web3'])) {
      this.web3 = (window['web3']);
      this.web3.setProvider(window['web3'].currentProvider);
    }
  }


  async tryIt() {

    if (this.web3 != null) {
      const from = this.getAccount();
      const message = this.web3.toHex('Hello world!');

      // window.localStorage.clear();
      // console.log(window.localStorage);
      // console.log(window.localStorage[from]);

      if (!isNullOrUndefined(window.localStorage[from])) {
        await this.navigateTo('/loggedin');
        return;
      }

      if (this.isMetaMask()) {
        this.web3.personal.sign(message, from, async (error, signedMessage) => {
          if (!isNullOrUndefined(signedMessage)) {

            window.localStorage['from'] = from;
            window.localStorage['message'] = message;
            window.localStorage['signedMessage'] = signedMessage;
            console.log(window.localStorage);
            // check signature

            await this.navigateTo('/loggedin');
          } else {
            console.log('Error: ' + error);
          }
        });
      } else {
        await this.navigateTo('/metamaskhowto');
      }
    }
  }

  private isMetaMask() {
    return this.web3.currentProvider.constructor.name === 'MetamaskInpageProvider';
  }

  private getAccount() {
    return this.web3.eth.accounts[0];
  }

  private async navigateTo(path: string) {
    this.ngZone.run(async () => {
      await this.router.navigate([path]);
    });
  }

  gotoApi() {


    let headers = new HttpHeaders();
    headers = headers
      .append('X-New-Eth-Address', localStorage['from'])
      .append('X-New-Signed-Message', localStorage['signedMessage'])
      .append('X-New-Message', localStorage['message']);


    const options = { headers: headers };

    this.http
      .get('http://localhost:5000/api/test/', options)
      .subscribe(
        data => {
          alert(data);
        },
        err => {
          alert(err);
        });
  }
}
