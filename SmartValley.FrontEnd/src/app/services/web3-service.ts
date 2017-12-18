import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import * as EthJs from 'ethjs';
import {NgProgress} from 'ngx-progressbar';
import {PromiseUtils} from '../utils/promise-utils';

@Injectable()
export class Web3Service {
  constructor(private progress: NgProgress) {
  }

  private readonly rinkebyNetworkId = '4';
  private readonly metamaskProviderName = 'MetamaskInpageProvider';
  private _transactionReceiptPollingInterval = 1000;

  private _eth: EthJs;

  get eth(): EthJs {
    if (isNullOrUndefined(this._eth)) {
      this._eth = new EthJs(this.getProvider());
    }
    return this._eth;
  }

  private getProvider() {
    if (this.isMetamaskInstalled) {
      return window['web3'].currentProvider;
    }
  }

  public get isMetamaskInstalled(): boolean {
    return (!isNullOrUndefined(window['web3']) && window['web3'].currentProvider.constructor.name === this.metamaskProviderName);
  }

  public getAccounts(): Promise<Array<string>> {
    return this.eth.accounts();
  }

  public sign(message: string, address: string): Promise<string> {
    return this.eth.personal_sign(EthJs.fromUtf8(message), address);
  }

  public fromWei(weiNumber: number, unit: string | number): number {
    if (typeof unit === 'number') {
      return weiNumber * Math.pow(10, -unit);
    }

    return EthJs.fromWei(weiNumber, unit);
  }

  public recoverSignature(message: string, signature: string): Promise<string> {
    return this.eth.personal_ecRecover(EthJs.fromUtf8(message), signature);
  }

  public async checkRinkebyNetworkAsync(): Promise<boolean> {
    const version = await this.getNetworkVersion();
    return version === this.rinkebyNetworkId;
  }

  public getContract(abiString: string, address: string) {
    const abi = JSON.parse(abiString);
    return this.eth.contract(abi).at(address);
  }

  public async waitForConfirmationAsync(transactionHash: string): Promise<void> {
    if (!this.progress.isStarted()) {
      this.progress.start();
    }

    let transactionReceipt = await this._eth.getTransactionReceipt(transactionHash);
    while (!transactionReceipt) {
      await PromiseUtils.delay(this._transactionReceiptPollingInterval);
      transactionReceipt = await this._eth.getTransactionReceipt(transactionHash);
    }
    this.progress.done();
    const status = parseInt(transactionReceipt.status, 16);
    if (status !== 1) {
      throw new Error(`Transaction '${transactionHash}' failed.`);
    }
  }

  public async getHash(value: string): Promise<string> {
    return await this.eth.web3_sha3(EthJs.fromUtf8(value));
  }



  private getNetworkVersion(): Promise<any> {
    return this.eth.net_version();
  }
}
