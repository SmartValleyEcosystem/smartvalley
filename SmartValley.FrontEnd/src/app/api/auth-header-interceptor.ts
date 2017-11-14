import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {AuthenticationService} from '../services/authentication-service';
import {HeadersConstants} from '../constants';
import {Web3Service} from '../services/web3-service';
import {UserInfo} from '../services/user-info';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/mergeMap';

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private web3Service: Web3Service) {
  }


  private getUserInfoAsObservable(): Observable<UserInfo> {
    return Observable.fromPromise(this.authenticationService.getUserInfo());
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return this.getUserInfoAsObservable()
      .mergeMap((user: UserInfo) => {
        let headers = new HttpHeaders();
        if (user != null) {
          headers = req.headers
            .append(HeadersConstants.XEthereumAddress, user.ethereumAddress)
            .append(HeadersConstants.XSignature, user.signature)
            .append(HeadersConstants.XSignedText, AuthenticationService.MESSAGE_TO_SIGN);
        }
        const request = user != null ? req.clone({headers: headers}) : req;
        return next.handle(request);
      });

  }
}

