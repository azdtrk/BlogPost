import { Injectable } from '@angular/core';
import { HttpService } from './http/http.service';
import { Observable, throwError } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export abstract class BaseService<T> {
  protected abstract apiPath: string;

  constructor(protected httpService: HttpService) {}

  getAll(page: number = 0, size: number = 10, params?: HttpParams): Observable<T[]> {
    let httpParams = params || new HttpParams();
    httpParams = httpParams
      .set('page', page.toString())
      .set('size', size.toString());

    return this.httpService.get<any>(this.apiPath, httpParams).pipe(
      map(response => this.normalizeResponse(response)),
      catchError(error => this.handleError('Failed to fetch items', error))
    );
  }

  getById(id: string): Observable<T> {
    if (!id) {
      return throwError(() => new Error('Entity ID is required'));
    }

    return this.httpService.get<any>(`${this.apiPath}/${id}`).pipe(
      map(response => this.normalizeResponse(response, true)),
      catchError(error => this.handleError(`Failed to fetch item with ID ${id}`, error))
    );
  }

  create(entity: Partial<T>): Observable<T> {
    return this.httpService.post<any>(this.apiPath, entity).pipe(
      map(response => this.normalizeResponse(response, true)),
      catchError(error => this.handleError('Failed to create item', error))
    );
  }

  update(id: string, entity: Partial<T>): Observable<T> {
    if (!id) {
      return throwError(() => new Error('Entity ID is required'));
    }
    return this.httpService.put<any>(`${this.apiPath}/${id}`, entity).pipe(
      map(response => this.normalizeResponse(response, true)),
      catchError(error => this.handleError(`Failed to update item with ID ${id}`, error))
    );
  }

  delete(id: string): Observable<void> {
    if (!id) {
      return throwError(() => new Error('Entity ID is required'));
    }
    return this.httpService.delete<void>(`${this.apiPath}/${id}`).pipe(
      catchError(error => this.handleError(`Failed to delete item with ID ${id}`, error))
    );
  }

  protected normalizeResponse(response: any, isSingleItem: boolean = false): any {
    if (!response) {
      return isSingleItem ? null : [];
    }

    if (isSingleItem) {
      if (response.Value) return response.Value;
      if (response.value) return response.value;
      if (response.data) return response.data;
      return response;
    }

    let items: any[] = [];

    if (Array.isArray(response)) {
      items = response;
    } else if (response?.Value && Array.isArray(response.Value)) {
      items = response.Value;
    } else if (response?.value && Array.isArray(response.value)) {
      items = response.value;
    } else if (response?.data && Array.isArray(response.data)) {
      items = response.data;
    } else if (response?.items && Array.isArray(response.items)) {
      items = response.items;
    } else {
      return [];
    }

    return items;
  }

  protected handleError(message: string, error: any): Observable<never> {
    return throwError(() => new Error(message));
  }
}
