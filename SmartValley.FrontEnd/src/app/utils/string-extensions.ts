import {isNullOrUndefined} from 'util';

export class StringExtensions {
  public static nullIfEmpty(value: string): string {
    return isNullOrUndefined(value) || value.length === 0 ? null : value;
  }
}
