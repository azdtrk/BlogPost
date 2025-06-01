import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment'
import { User } from '../models/user.model';
import { LoginRequest, RegisterRequest} from '../models/auth.model';
import { Router } from '@angular/router';
import { AuthBaseService } from './auth-base.service';
import { HttpService } from './http/http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends AuthBaseService<User> {
  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser$: Observable<User | null>;
  private jwtHelper: JwtHelperService;

  protected override apiPath = '/api/auth';
  protected override tokenKey = 'token';

  constructor(
    httpService: HttpService,
    private router: Router
  ) {
    super(httpService);
    this.jwtHelper = new JwtHelperService();
    this.currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromToken());
    this.currentUser$ = this.currentUserSubject.asObservable();
  }

  override login(credentials: LoginRequest): Observable<any> {
    return super.login(credentials).pipe(
      tap(response => {
        this.handleAuthentication(response);
      })
    );
  }

  override register(userData: RegisterRequest): Observable<any> {
    const requestData = {
      username: userData.username,
      email: userData.email,
      password: userData.password,
      role: userData.role || 'reader'
    };

    return super.register(requestData).pipe(
      tap(response => {
        this.handleAuthentication(response);
      })
    );
  }

  logout(): void {
    this.removeToken();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  override isAuthenticated(): boolean {
    const token = this.getToken();
    try {
      return token != null && !this.jwtHelper.isTokenExpired(token);
    } catch (error) {
      this.removeToken();
      return false;
    }
  }

  override getCurrentUser(): User | null {
    return this.currentUserSubject.value;
  }

  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    if (!user || !user.role)
      return false;

    if (Array.isArray(user.role)) {
      return user.role.some(r => r.toLowerCase() === role.toLowerCase());
    }

    return user.role.toLowerCase() === role.toLowerCase();
  }

  isAdmin(): boolean {
    const user = this.getCurrentUser();
    return user?.role?.toLowerCase() === 'author';
  }

  isReader(): boolean {
    const user = this.getCurrentUser();
    return user?.role?.toLowerCase() === 'reader';
  }

  redirectToDashboard(): void {
    if (this.isAdmin()) {
      this.router.navigate(['/admin']);
    } else {
      this.router.navigate(['/user']);
    }
  }

  private handleAuthentication(response: any): void {

    if (!response)
      return;

    if (response.IsSuccess === undefined && response.succeeded === undefined)
      return;

    const isSuccess = response.IsSuccess !== undefined ? response.IsSuccess : response.succeeded;
    const userData = response.Value || response.data;

    if (!isSuccess || !userData)
      return;

    // Extract token from different possible formats
    let token = null;
    if (userData.token && userData.token.accessToken) {
      token = userData.token.accessToken;
    } else if (userData.Token && userData.Token.AccessToken) {
      token = userData.Token.AccessToken;
    } else if (userData.token) {
      token = userData.token;
    } else if (userData.Token) {
      token = userData.Token;
    } else {
      return;
    }

    if (!token)
      return;

    this.saveToken(token);

    const user: User = {
      id: userData.Id || userData.id,
      username: userData.UserName || userData.userName,
      email: (userData.UserName || userData.userName || '').includes('@') ? (userData.UserName || userData.userName) : '',
      role: userData.Role !== undefined ? (typeof userData.Role === 'number' ? this.getRoleNameFromId(userData.Role) : userData.Role) : userData.role
    };

    this.currentUserSubject.next(user);
  }

  private getRoleNameFromId(roleId: number): string {
    switch(roleId) {
      case 0: return 'author';
      case 1: return 'reader';
      default: return 'reader';
    }
  }

  private getUserFromToken(): User | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      if (this.jwtHelper.isTokenExpired(token)) {
        this.removeToken();
        return null;
      }

      const decodedToken = this.jwtHelper.decodeToken(token);

      const userId = decodedToken.nameid ||
                    decodedToken.sub ||
                    decodedToken.Id ||
                    decodedToken.id ||
                    decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ||
                    decodedToken['nameidentifier'];

      const userName = decodedToken.unique_name ||
                      decodedToken.username ||
                      decodedToken.UserName ||
                      decodedToken.name ||
                      decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
                      decodedToken['name'];

      const userEmail = decodedToken.email ||
                       decodedToken.Email ||
                       (userName && userName.includes('@') ? userName : '') ||
                       decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
                       decodedToken['emailaddress'];

      let userRole = null;
      if (decodedToken.role !== undefined)
        userRole = decodedToken.role;
      else if (decodedToken.Role !== undefined)
        userRole = decodedToken.Role;
      else if (decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] !== undefined)
        userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      else if (decodedToken['role'] !== undefined)
        userRole = decodedToken['role'];

      if (userRole !== undefined && Array.isArray(userRole) && userRole.length > 0)
        userRole = userRole[0];

      if (userRole !== undefined && !isNaN(Number(userRole)))
        userRole = this.getRoleNameFromId(Number(userRole));

      if (!userRole && userId)
        userRole = 'reader';

      if (!userId) {
        this.removeToken();
        return null;
      }

      const user = {
        id: userId,
        username: userName || 'user',
        email: userEmail || 'user@example.com', // Ensure we always have some email as fallback
        role: userRole || 'reader', // Default to reader if no role specified
        firstName: decodedToken.given_name || decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'] || '',
        lastName: decodedToken.family_name || decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'] || ''
      };

      return user;
    } catch (error) {
      this.removeToken();
      return null;
    }
  }

  override handleError(message: string, error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      if (error.status === 0) {
        errorMessage = 'Cannot connect to the server. Please check your network connection.';
      } else if (error.status === 401) {
        errorMessage = 'Invalid credentials';
      } else if (error.status === 400) {
        if (error.error && typeof error.error === 'object') {
          if (error.error.message) {
            errorMessage = error.error.message;
          } else if (error.error.errors) {
            const validationErrors = Object.values(error.error.errors).flat();
            errorMessage = validationErrors.join('. ');
          }
        }
      } else if (error.status === 404) {
        errorMessage = 'The requested resource was not found';
      } else if (error.status === 500) {
        errorMessage = 'Server error occurred. Please try again later.';
      }
    }

    return throwError(() => new Error(errorMessage));
  }

  public validateAndRefreshTokenIfNeeded(): boolean {
    const token = this.getToken();

    if (!token) {
      this.router.navigate(['/login']);
      return false;
    }

    try {
      if (this.jwtHelper.isTokenExpired(token)) {
        this.logout();
        return false;
      }
      const decodedToken = this.jwtHelper.decodeToken(token);
      if (!decodedToken) {
        this.logout();
        return false;
      }
      return true;
    } catch (error) {
      this.removeToken();
      return false;
    }

  }

}
