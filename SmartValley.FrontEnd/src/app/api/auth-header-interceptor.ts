import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {AuthenticationService} from '../services/authentication/authentication-service';
import {HeadersConstants} from '../constants';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/mergeMap';

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService) {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const user = this.authenticationService.getCurrentUser();
    if (user == null) {
      return next.handle(req);
    }

    const headers = req.headers
      .append(HeadersConstants.XEthereumAddress, user.account)
      .append(HeadersConstants.XSignature, user.signature)
      .append(HeadersConstants.XSignedText, AuthenticationService.MESSAGE_TO_SIGN);

    const request = req.clone({headers: headers});
    return next.handle(request);
  }
}

