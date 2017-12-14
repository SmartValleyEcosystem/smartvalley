export class BaseContractClient {

  public extractNumberValue(result: Array<any>): number {
    return +result[0].toString(10);
  }

  public extractBoolValue(result: Array<any>): boolean {
    return result[0];
  }
}
