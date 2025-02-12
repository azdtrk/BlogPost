import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpService } from './http/http.service';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiPath = '/api/users';

  constructor(private httpService: HttpService) {}

  getProfile(): Observable<User> {
    return this.httpService.get<User>(`${this.apiPath}/profile`);
  }

  updateProfile(userData: Partial<User>): Observable<User> {
    return this.httpService.put<User>(`${this.apiPath}/profile`, userData);
  }

  getById(id: number): Observable<User> {
    return this.httpService.get<User>(`${this.apiPath}/${id}`);
  }
} 