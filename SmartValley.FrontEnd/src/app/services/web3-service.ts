import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import * as EthJs from 'ethJs';


@Injectable()
export class Web3Service {

  public static MESSAGE_TO_SIGN = 'Confirm login';

  private readonly rinkebyNetworkId = '4';
  // TODO next task
  private readonly metamaskProviderName = 'MetamaskInpageProvider';


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

  public isAddress(address: string): boolean {
    return EthJs.isAddress(address);
  }

  public async getAccounts(): Promise<Array<string>> {
    return await this.eth.accounts();
  }

  public async sign(address: string): Promise<string> {
    return await this.eth.personal_sign(EthJs.fromUtf8(Web3Service.MESSAGE_TO_SIGN), address);
  }

  public async recoverSignature(signature: string): Promise<string> {
    return this.eth.personal_ecRecover(EthJs.fromUtf8(Web3Service.MESSAGE_TO_SIGN), signature);
  }

  public async isRinkebyNetwork(): Promise<boolean> {
    const version = await this.getNetworkVersion();
    return version === this.rinkebyNetworkId;
  }

  private async getNetworkVersion(): Promise<any> {
    return await this.eth.net_version();
  }

}
