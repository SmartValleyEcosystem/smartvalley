import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {AuthenticationService} from '../services/authentication-service';
import {Constants} from '../constants';
import {Web3Service} from '../services/web3-service';
import {UserInfo} from '../services/user-info';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/mergeMap';

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private web3Service: Web3Service) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return this.authenticationService.getUserInfoAsObservable()
      .mergeMap((user: UserInfo) => {

        const headers = req.headers
          .append(Constants.XEthereumAddress, user != null ? user.ethereumAddress : null)
          .append(Constants.XSignature, user != null ? user.signature : null)
          .append(Constants.XSignedText, AuthenticationService.MESSAGE_TO_SIGN);
        const request = user != null ? req.clone({headers: headers}) : req;
        return next.handle(request);
      });

  }
}

