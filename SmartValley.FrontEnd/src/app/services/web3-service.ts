import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import * as EthJs from 'ethjs';
import {NgProgress} from 'ngx-progressbar';
import {PromiseUtils} from '../utils/promise-utils';
import BigNumber from 'bignumber.js';
import {environment} from '../../environments/environment';

@Injectable()
export class Web3Service {
  constructor(private progress: NgProgress) {
  }

  private readonly networkId = environment.network_id;
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
    return (!isNullOrUndefined(window['web3']) && window['web3'].currentProvider.isMetaMask);
  }

  public async getCurrentAccountAsync(): Promise<string> {
    const accounts = await this.eth.accounts();
    return accounts[0];
  }

  public signAsync(message: string, address: string): Promise<string> {
    return this.eth.personal_sign(EthJs.fromUtf8(message), address);
  }

  public fromWei(weiNumber: number): number {
    return EthJs.fromWei(weiNumber, 'ether');
  }

  public recoverSignature(message: string, signature: string): Promise<string> {
    return this.eth.personal_ecRecover(EthJs.fromUtf8(message), signature);
  }

  public async checkNetworkAsync(): Promise<boolean> {
    const version = await this.getNetworkVersionAsync();
    return version === this.networkId;
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

  private getNetworkVersionAsync(): Promise<any> {
    return this.eth.net_version();
  }
}
