import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment } from '../models/comment.model';
import { BaseService } from './base.service';
import { HttpService } from './http/http.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService extends BaseService<Comment> {
  protected apiPath = '/api/comments';

  constructor(httpService: HttpService) {
    super(httpService);
  }

  // Custom method specific to Comment
  getByPostId(postId: number): Observable<Comment[]> {
    return this.httpService.get<Comment[]>(`${this.apiPath}/post/${postId}`);
  }
} 