import {Injectable} from '@angular/core';
import * as blockies from 'blockies';

@Injectable()
export class BlockiesService {
  public getImageForAddress(address: string): string {
    return blockies({
      seed: address.toLowerCase(),
      size: 8,
      scale: 16
    }).toDataURL();
  }
}
