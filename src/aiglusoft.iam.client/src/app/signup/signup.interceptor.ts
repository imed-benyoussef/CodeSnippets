import { HttpEvent, HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { exhaustMap, switchMap, take } from 'rxjs/operators';
import { selectAccessToken } from './store/selectors/signup.selectors';
import { Observable } from 'rxjs';



@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private store: Store) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.store.pipe(select(selectAccessToken)).pipe(
      take(1), // Ensure we get the current token only once
      switchMap(accessToken => {
        if (!accessToken) {
          return next.handle(req);
        }

        const clonedReq = req.clone({
          setHeaders: {
            Authorization: `Bearer ${accessToken}`
          }
        });
        return next.handle(clonedReq);
      })
    );
  }
}