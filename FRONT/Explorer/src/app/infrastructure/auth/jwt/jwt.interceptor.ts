import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ACCESS_TOKEN } from '../../../shared/constants';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessTokenRequest = request.clone({
      setHeaders: {
        Authorization: `Bearer ` + localStorage.getItem(ACCESS_TOKEN),
      },
    });
    return next.handle(accessTokenRequest);
  }
}