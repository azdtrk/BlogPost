import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  get<T>(path: string, params?: HttpParams): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}${path}`, { params });
  }

  post<T>(path: string, body: any): Observable<T> {
    // No need to set the content type for FormData, browser will set it automatically with boundary
    if (body instanceof FormData) {
      return this.http.post<T>(`${this.apiUrl}${path}`, body);
    }
    
    return this.http.post<T>(`${this.apiUrl}${path}`, body, {
      headers: new HttpHeaders().set('Content-Type', 'application/json')
    });
  }

  put<T>(path: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}${path}`, body);
  }

  delete<T>(path: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}${path}`);
  }
}