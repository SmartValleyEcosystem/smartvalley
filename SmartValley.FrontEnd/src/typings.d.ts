/* SystemJS module definition */
declare var module: NodeModule;

interface NodeModule {
  id: string;
}

/* SystemJS module definition */


declare var ethJs: EthJs;

declare class EthJs {

  constructor(provider: any);

  static fromUtf8(str: string): string;

  static isAddress(address: string): boolean;

  personal_sign(address: string, message: string): Promise<string>;

  accounts(): Promise<Array<string>>;

  net_version(): Promise<string>;

  personal_ecRecover(signature: string, message: string): Promise<string>;
}

declare function isNullOrEmpty(str: string);

