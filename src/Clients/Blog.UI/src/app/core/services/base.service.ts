import { Injectable } from '@angular/core';
import { HttpService } from './http/http.service';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable()
export abstract class BaseService<T> {
  protected abstract apiPath: string;

  constructor(protected httpService: HttpService) {}

  getAll(params?: HttpParams): Observable<T[]> {
    return this.httpService.get<T[]>(this.apiPath, params);
  }

  getById(id: number): Observable<T> {
    return this.httpService.get<T>(`${this.apiPath}/${id}`);
  }

  create(entity: Partial<T>): Observable<T> {
    return this.httpService.post<T>(this.apiPath, entity);
  }

  update(id: number, entity: Partial<T>): Observable<T> {
    return this.httpService.put<T>(`${this.apiPath}/${id}`, entity);
  }

  delete(id: number): Observable<void> {
    return this.httpService.delete<void>(`${this.apiPath}/${id}`);
  }
} 