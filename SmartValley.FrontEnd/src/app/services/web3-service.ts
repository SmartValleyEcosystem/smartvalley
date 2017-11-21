import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';
import * as EthJs from 'ethJs';

@Injectable()
export class Web3Service {

  private readonly rinkebyNetworkId = '4';
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

  public getAccounts(): Promise<Array<string>> {
    return this.eth.accounts();
  }

  public sign(message: string, address: string): Promise<string> {
    return this.eth.personal_sign(EthJs.fromUtf8(message), address);
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
    return this._eth.contract(abi).at(address);
  }

  private getNetworkVersion(): Promise<any> {
    return this.eth.net_version();
  }
}
