// import { Injectable } from '@angular/core';
// import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
// import { Observable } from 'rxjs';
// import { AuthService } from '../authentication/services/auth.service';

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {
//   constructor(private authService: AuthService) {}

//   intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//     const token = this.authService.getToken() || localStorage.getItem('token');
//     if (token) {
//       console.log(token);
//       const clonedRequest = req.clone({
//         headers: req.headers.set('Authorization', `Bearer ${token}`)
//       });
//       console.log(next.handle(clonedRequest));
//       return next.handle(clonedRequest);
//     }
//     return next.handle(req);
//   }
// }

import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('token');
  if (token) {
    const cloneReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloneReq);
  }
  return next(req);
};