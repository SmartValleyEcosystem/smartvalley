import BigNumber from 'bignumber.js';

export class ConverterHelper {
  static extractNumberValue(result): number {
    return +result[0].toString(10);
  }

  static extractBoolValue(result): boolean {
    return result[0];
  }

  static extractStringValue(result): string {
    return result[0];
  }

  static extractBigNumber(result): BigNumber {
    return new BigNumber(result[0], 10);
  }
}
