import { Injectable } from '@angular/core';
import { HttpService } from './http/http.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { User } from '../models/user.model';

@Injectable()
export abstract class AuthBaseService<T> {
  protected abstract apiPath: string;
  protected abstract tokenKey: string;

  constructor(protected httpService: HttpService) {}

  login(credentials: any): Observable<any> {
    return this.httpService.post<any>(`${this.apiPath}/login`, credentials).pipe(
      catchError(error => this.handleError('Login failed', error))
    );
  }

  register(userData: any): Observable<any> {
    return this.httpService.post<any>(`${this.apiPath}/register`, userData).pipe(
      catchError(error => this.handleError('Registration failed', error))
    );
  }

  refreshToken(token: string): Observable<any> {
    return this.httpService.post<any>(`${this.apiPath}/refresh-token`, { token }).pipe(
      catchError(error => this.handleError('Token refresh failed', error))
    );
  }

  abstract isAuthenticated(): boolean;

  abstract getCurrentUser(): User | null;

  protected saveToken(token: string): void {
    if (!token)
      return;
    localStorage.setItem(this.tokenKey, token);
  }

  public getToken(): string | null {
    const token = localStorage.getItem(this.tokenKey);
    if (!token)
      return null;
    return token;
  }

  protected removeToken(): void {
    localStorage.removeItem(this.tokenKey);
  }

  protected handleError(message: string, error: any): Observable<never> {
    return throwError(() => new Error(message));
  }
}
