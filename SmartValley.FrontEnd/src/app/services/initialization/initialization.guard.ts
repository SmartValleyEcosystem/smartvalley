import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {InitializationService} from './initialization.service';
import {Paths} from '../../paths';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/pairwise';

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
