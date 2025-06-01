import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Error: ${error.error.message}`;
        } else {
          // Server-side error
          switch (error.status) {
            case 401:
              errorMessage = 'Unauthorized: Please log in to access this resource';
              this.router.navigate(['/auth'], { queryParams: { returnUrl: this.router.url } });
              break;
            case 403:
              errorMessage = 'Forbidden: You do not have permission to access this resource';
              break;
            case 404:
              errorMessage = 'Not Found: The requested resource was not found';
              break;
            case 400:
              errorMessage = error.error?.message || 'Bad Request: The request was invalid';
              break;
            case 500:
              errorMessage = 'Server Error: Something went wrong on the server';
              break;
            default:
              errorMessage = `Error ${error.status}: ${error.error?.message || error.statusText}`;
          }
        }
        return throwError(() => ({ message: errorMessage, status: error.status, error: error.error }));
      })
    );
  }
}
