export class ConverterHelper {
  static extractNumberValue(result: Array<any>): number {
    return +result[0].toString(10);
  }

  static extractBoolValue(result: Array<any>): boolean {
    return result[0];
  }
}
