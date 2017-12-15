import {Injectable} from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {InitializationService} from './initialization.service';
import {Paths} from '../../paths';

@Injectable()
export class InitializationGuard implements CanActivate {

  constructor(private initializationService: InitializationService,
              private router: Router) {
  }

  canActivate(next: ActivatedRouteSnapshot,
              state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    if (this.initializationService.isAppInitialized) {
      return true;
    }
    this.router.navigate([Paths.Initialization], {queryParams: {returnUrl: state.url}});
    return;
  }
}
