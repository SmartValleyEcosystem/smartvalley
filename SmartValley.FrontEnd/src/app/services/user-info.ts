export class UserInfo {
  constructor(ethereumAddress, signature, isAuthentiticated) {
    this.ethereumAddress = ethereumAddress;
    this.signature = signature;
    this.isAuthenticated = isAuthentiticated;
  }

  public ethereumAddress: string;
  public signature: string;
  public isAuthenticated: boolean;

  static anonymous(): UserInfo {
    return new UserInfo(null, null, false);
  }
}

