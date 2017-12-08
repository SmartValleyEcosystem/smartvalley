import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import {AuthenticationService} from '../services/authentication/authentication-service';
import {NotificationsService} from 'angular2-notifications';
import {TranslateService} from '@ngx-translate/core';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService,
              private notificationsService: NotificationsService,
              private translateService: TranslateService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).do((event: HttpEvent<any>) => {
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401 || err.status === 403) {
          this.authenticationService.stopUserSession();
        } else if (err.status === 500) {
          this.notificationsService.error(this.translateService.instant('Common.ServerError'), this.translateService.instant('Common.TryAgain'));
        }
      }
    });
  }
}

