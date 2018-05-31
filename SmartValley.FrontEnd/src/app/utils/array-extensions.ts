import {isNullOrUndefined} from 'util';

export class ArrayExtensions {
  // ... still no implementation ...
}

class CustomArray<T> extends Array<T> {
  // ... still no implementation ...
}

declare global {
  interface Array<T> {
    selectMany<U>(this: T[], selector: (x: T) => U[]): CustomArray<U>;

    first(this: T[], selector?: (x: T) => boolean): T;

    last(this: T[], selector?: (x: T) => boolean): T;

    firstOrDefault(this: T[], selector?: (x: T) => boolean): T;
  }
}

if (!Array.prototype.firstOrDefault) {
  Array.prototype.firstOrDefault = function (this, selector?: (x) => boolean) {
    if (isNullOrUndefined(selector)) {
      return this[0];
    }

    return this.filter(selector)[0];
  };
}

if (!Array.prototype.last) {
  Array.prototype.last = function (this, selector?: (x) => boolean) {
    if (isNullOrUndefined(selector)) {
      if (this.length < 1) {
        throw new Error('Sequence contains no elements');
      }
      return this[this.length - 1];
    } else {
      const value = this.filter(selector);
      if (value.length < 1) {
        throw new Error('Sequence contains no elements');
      }
      return value[value.length - 1];
    }
  };
}

if (!Array.prototype.first) {
  Array.prototype.first = function (this, selector?: (x) => boolean) {
    if (isNullOrUndefined(selector)) {
      if (this.length < 1) {
        throw new Error('Sequence contains no elements');
      }
      return this[0];
    } else {
      const value = this.filter(selector);
      if (value.length < 1) {
        throw new Error('Sequence contains no elements');
      }
      return value[0];
    }
  };
}

if (!Array.prototype.selectMany) {
  Array.prototype.selectMany = function <T, U>(this: T[], selector: (x: T) => U[]): CustomArray<U> {
    if (!this.length) {
      return new CustomArray<U>();
    }
    return this.map(selector).reduce((l, r) => l.concat(r));
  };
}
