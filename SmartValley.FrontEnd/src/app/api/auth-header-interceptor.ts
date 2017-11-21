import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {AuthenticationService} from '../services/authentication-service';
import {HeadersConstants} from '../constants';
import {Web3Service} from '../services/web3-service';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/mergeMap';

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthenticationService, private web3Service: Web3Service) {
  }

// TODO next task
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req);
    // return this.getUserInfoAsObservable()
    //   .mergeMap((user: User) => {
    //     let headers = new HttpHeaders();
    //     if (user != null) {
    //       headers = req.headers
    //         .append(HeadersConstants.XEthereumAddress, user.account)
    //         .append(HeadersConstants.XSignature, user.signature)
    //         .append(HeadersConstants.XSignedText, Web3Service.MESSAGE_TO_SIGN);
    //     }
    //     const request = user != null ? req.clone({headers: headers}) : req;
    //     return next.handle(request);
    //   });

  }
}

