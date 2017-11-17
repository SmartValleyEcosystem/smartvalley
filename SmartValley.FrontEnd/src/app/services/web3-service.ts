import {Injectable} from '@angular/core';
import {isNullOrUndefined} from 'util';


@Injectable()
export class Web3Service {

  constructor() {
    this.initialize();
  }

  public static MESSAGE_TO_SIGN = 'Confirm login';

  public isConnected(){
    return this.eth.
  }

  private get metamaskProvider(): any {
    if (!isNullOrUndefined(window['web3']) && window['web3'].currentProvider) {
      return window['web3'].currentProvider;
    }
  }

  private readonly rinkebyNetworkId = '4';
  private metamaskProviderName = 'MetamaskInpageProvider';
  private eth: EthJs;
  private isExtensionEnabled = !isNullOrUndefined(window['web3']);


  private initialize() {
    this.eth = new EthJs(this.metamaskProvider);
  }

  public isAddress(address: string): boolean {
    return EthJs.isAddress(address);
  }

  public async getAccounts(): Promise<Array<string>> {
    return await this.eth.accounts();
  }

  public async signInToMetamask(address: string): Promise<string> {
    return await this.eth.personal_sign(EthJs.fromUtf8(Web3Service.MESSAGE_TO_SIGN), address);
  }

  public async isRinkebyNetwork(): Promise<boolean> {
    const version = await this.getNetworkVersion();
    return version === this.rinkebyNetworkId;
  }

  private async getNetworkVersion(): Promise<any> {
    return await this.eth.net_version();
  }
}
