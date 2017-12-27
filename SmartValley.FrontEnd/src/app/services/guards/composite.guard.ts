import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {GuardFactory} from './guard-factory';
import {Observable} from 'rxjs/Observable';

@Injectable()
export class CompositeGuard implements CanActivate {
  constructor(private guardFactory: GuardFactory) {
  }

  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return this.executeGuards(route, state);
  }

  private executeGuards(route: ActivatedRouteSnapshot, state: RouterStateSnapshot, guardIndex: number = 0): Promise<boolean> {
    return this.activateGuardAsync(route, state, guardIndex)
      .then(result => {
        if (!result) {
          return Promise.resolve(false);
        }
        if (guardIndex < route.data.guards.length - 1) {
          return this.executeGuards(route, state, guardIndex + 1);
        } else {
          return Promise.resolve(true);
        }
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  private activateGuardAsync(route: ActivatedRouteSnapshot, state: RouterStateSnapshot, guardIndex: number): Promise<boolean> {
    const guard = this.guardFactory.create(route.data.guards[guardIndex]);
    const result = guard.canActivate(route, state);

    return this.toPromise(result);
  }

  private toPromise(value: Promise<boolean> | Observable<boolean> | boolean): Promise<boolean> {
    if (value instanceof Promise) {
      return value;
    }
    if (value instanceof Observable) {
      return value.toPromise<boolean>();
    }
    return Promise.resolve(value);
  }
}
