import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {Constants, HeadersConstants} from '../constants';
import {UserContext} from '../services/authentication/user-context';
import {isNullOrUndefined} from 'util';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private userContext: UserContext) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const user = this.userContext.getCurrentUser();
    if (isNullOrUndefined(user) || isNullOrUndefined(user.token)) {
      return next.handle(req);
    }

    const headers = req.headers
      .append(HeadersConstants.Authorization, 'Bearer ' + user.token);
    const request = req.clone({headers: headers});
    return next.handle(request).do((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        const newJwt = event.headers.get(HeadersConstants.XNewAuthToken);
        if (!isNullOrUndefined(newJwt)) {
          user.token = newJwt;
          user.roles = event.headers.get(HeadersConstants.XNewRoles).split(',');
          this.userContext.saveCurrentUser(user);
        }
      }
    });
  }
}
