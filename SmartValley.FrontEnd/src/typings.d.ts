/* SystemJS module definition */
declare var module: NodeModule;

interface NodeModule {
  id: string;
}


declare var ethJs: EthJs;

declare class EthJs {

  constructor(provider: any);

  static fromUtf8(str: string): string;

  static fromWei(wei: string, unit: string): number;

  static toWei(value: number, unit: string);

  static isAddress(address: string): boolean;

  personal_sign(address: string, message: string): Promise<string>;

  accounts(): Promise<Array<string>>;

  net_version(): Promise<string>;

  personal_ecRecover(message: string, signature: string): Promise<string>;
}

declare var System: System;

interface System {
  import(request: string): Promise<any>;
}

