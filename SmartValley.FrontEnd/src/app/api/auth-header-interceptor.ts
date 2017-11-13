import {HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {LoginInfoService} from "../services/login-info-service";
import {Constants} from "../constants";

@Injectable()
export class AuthHeaderInterceptor implements HttpInterceptor {
  constructor(private loginInfoService: LoginInfoService) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let headers = new HttpHeaders();
    headers = headers
      .append(Constants.XEthereumAddress, localStorage[LoginInfoService.ETH_ADDRESS_KEY])
      .append(Constants.XSignature, localStorage[LoginInfoService.SIGNATURE_KEY])
      .append(Constants.XSignedText, LoginInfoService.MESSAGE_TO_SIGN);
    const request = req.clone({headers: headers});
    return next.handle(request);
  }
}
