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
import { environment } from '../../environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Check if the request is to our API
    if (request.url.includes(environment.apiUrl)) {
      const token = this.authService.getToken();
      
      if (token) {
        // Special handling for image upload to avoid modifying Content-Type
        const isImageUpload = request.url.includes('/upload-image');
        
        if (isImageUpload && request.body instanceof FormData) {
          // Only add Authorization header without changing Content-Type for FormData
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
        } else {
          // For regular JSON requests, set both headers
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`,
              'Content-Type': 'application/json'
            }
          });
        }
      }
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // Special handling for image upload 401 errors
        const isImageUpload = request.url.includes('/upload-image');
        
        if (error.status === 401 && !isImageUpload) {
          this.authService.logout();
          this.router.navigate(['/login']);
        }
        
        return throwError(() => error);
      })
    );
  }
} 