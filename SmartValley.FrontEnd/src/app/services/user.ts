export class User {
  constructor(ethereumAddress, signature) {
    this.account = ethereumAddress;
    this.signature = signature;
  }

  public account: string;
  public signature: string;
}
