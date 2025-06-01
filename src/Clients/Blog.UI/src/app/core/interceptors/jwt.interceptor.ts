import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (request.url.includes('/api/BlogPost/upload-image')) {
      return next.handle(request);
    }

    if (request.url.startsWith('/api')) {
      const token = this.authService.getToken();

      const isLoginRequest = request.url.includes('/auth/login');
      const isFormData = request.body instanceof FormData;

      if (token) {
        const headers: { [key: string]: string } = {};

        headers['Authorization'] = `Bearer ${token}`;

        if (!isLoginRequest && !isFormData) {
          headers['Content-Type'] = 'application/json';
        }

        request = request.clone({
          setHeaders: headers
        });
      } else if (!isLoginRequest && !isFormData) {
        request = request.clone({
          setHeaders: {
            'Content-Type': 'application/json'
          }
        });
      }
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          localStorage.removeItem('token');
          this.authService.logout();
          const currentUrl = this.router.url;
          localStorage.setItem('redirectUrl', currentUrl);
          this.router.navigate(['/login'], {
            queryParams: {
              returnUrl: currentUrl,
              expired: 'true'
            }
          });
        }
        return throwError(() => error);
      })
    );
  }
}
