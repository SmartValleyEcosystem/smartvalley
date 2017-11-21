import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import * as EthJs from 'ethJs';


@Injectable()
export class Web3Service {

  constructor() {
    this.initialize();
  }

  private readonly rinkebyNetworkId = '4';
  // TODO next task
  // private metamaskProviderName = 'MetamaskInpageProvider';
  private eth: EthJs;

  private initialize() {
    this.eth = new EthJs(this.getProvider());
  }

  private getProvider() {
    if (!isNullOrUndefined(window['web3']) && window['web3'].currentProvider) {
      return window['web3'].currentProvider;
    }
  }


  public isAddress(address: string): boolean {
    return EthJs.isAddress(address);
  }

  public getAccounts(): Promise<Array<string>> {
    return this.eth.accounts();
  }

  public sign(message: string, address: string): Promise<string> {
    return this.eth.personal_sign(EthJs.fromUtf8(message), address);
  }

  public recoverSignature(message: string, signature: string): Promise<string> {
    return this.eth.personal_ecRecover(EthJs.fromUtf8(message), signature);
  }

  public async isRinkebyNetwork(): Promise<boolean> {
    const version = await this.getNetworkVersion();
    return version === this.rinkebyNetworkId;
  }

  private getNetworkVersion(): Promise<any> {
    return this.eth.net_version();
  }

}
