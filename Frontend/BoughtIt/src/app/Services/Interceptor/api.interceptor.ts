import { Injectable } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HttpHandlerFn } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter, take } from 'rxjs/operators';
import { inject } from '@angular/core';
import { UserService } from '../UserService/user.service';

export const APIInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(UserService);  
  let isRefreshing = false;
  const refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  let authToken = authService.getUserToken();
  if (authToken) {
    req = addTokenHeader(req, authToken);
  }
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isRefreshing) {
        return handle401Error(req, next, authService, refreshTokenSubject);
      }
      return throwError(() => error);
    })
  );
};

function addTokenHeader(request: HttpRequest<any>, token: string) {
  return request.clone({
    setHeaders: { Authorization: `Bearer ${token}` }
  });
}

function handle401Error(
  req: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: UserService,
  refreshTokenSubject: BehaviorSubject<any>
): Observable<HttpEvent<any>> {
 
    refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      switchMap((token: any) => {
        authService.saveAccessToken(token.accessToken);
        refreshTokenSubject.next(token.accessToken);
        return next(addTokenHeader(req, token.accessToken));
      }),
      catchError((err) => {
        console.log(err);
        authService.logout();
        return throwError(() => err);
      })
    );
}
