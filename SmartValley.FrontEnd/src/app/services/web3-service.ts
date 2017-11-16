import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import "rxjs/add/observable/timer";

@Injectable()
export class Web3Service {

  private rinkebyNetworkId = '4';
  private metamaskProviderName = 'MetamaskInpageProvider';

  private web3: any;
  private _isInitialized = false;

  get isInitialized(): boolean {
    return this._isInitialized;
  }

  public initialize(): void {
    if (typeof window['web3'] !== 'undefined') {
      this.web3 = new this.Web3(window['web3'].currentProvider);
      this._isInitialized = this.isMetaMask();
    }
  }

  public async sign(message: string, from: string): Promise<string> {
    const signature = await this.getSignature(message, from);
    const isSignatureCorrect = await this.checkSignature(signature, from, message);
    if (!isSignatureCorrect) {
      throw Error('Message signature is incorrect.');
    }

    return signature;
  }

  private getSignature(message: string, from: string): Promise<string> {
    if (!this.isInitialized) {
      return Promise.reject('Check your metamask please');
    }
    return new Promise<string>((resolve, reject) => {
      this.web3.personal.sign(this.web3.toHex(message), from, (err, result) => {
        if (!isNullOrUndefined(result)) {
          resolve(result);
        } else {
          reject('Message signing failed: ' + err);
        }
      });
    });
  }

  public getAccounts(): Promise<Array<string>> {
    return new Promise<Array<string>>((resolve, reject) => {
      this.web3.eth.getAccounts((err, accounts) => {
        if (!isNullOrUndefined(accounts)) {
          resolve(accounts);
        } else {
          reject('No accounts found: ' + err);
        }
      });
    });
  }

  public isRinkebyNetwork(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.web3.version.getNetwork((err, networkId) => {
        if (!isNullOrUndefined(networkId)) {
          resolve(networkId === this.rinkebyNetworkId);
        } else {
          reject('Network check failed: ' + err);
        }
      });
    });
  }

  public checkSignature(signedMessage: string, account: string, originalMessage: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.web3.personal.ecRecover(this.web3.toHex(originalMessage), signedMessage, (err, result) => {
        if (!isNullOrUndefined(result)) {
          resolve(result === account);
        } else {
          reject('Message signature check failed: ' + err);
        }
      });
    });
  }

  private isMetaMask(): boolean {
    return this.web3.currentProvider.constructor.name === this.metamaskProviderName;
  }

  get Web3(): any {
    return window['Web3'];
  }
}
