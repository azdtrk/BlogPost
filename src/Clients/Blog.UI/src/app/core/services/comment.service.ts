import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Comment } from '../models/comment.model';
import { BaseService } from './base.service';
import { HttpService } from './http/http.service';

@Injectable({
  providedIn: 'root'
})
export class CommentService extends BaseService<Comment> {
  protected override apiPath = '/api/comments';

  constructor(httpService: HttpService) {
    super(httpService);
  }

  getByBlogPostId(blogPostId: string): Observable<Comment[]> {
    return this.httpService.get<Comment[]>(`${this.apiPath}/post/${blogPostId}`);
  }

  protected override normalizeResponse(response: any, isSingleItem: boolean = false): any {
    const normalizedData = super.normalizeResponse(response, isSingleItem);

    if (!normalizedData)
      return normalizedData;

    if (isSingleItem)
      return this.mapToComment(normalizedData);

    return normalizedData.map((comment: any) => this.mapToComment(comment));
  }

  private mapToComment(comment: any): Comment {
    return {
      id: comment.id || comment.Id,
      content: comment.content || comment.Content,
      blogPostId: comment.blogPostId || comment.BlogPostId,
      authorId: comment.authorId || comment.AuthorId,
      author: comment.author || comment.Author,
      createdAt: comment.createdAt || comment.CreatedAt || comment.dateCreated || comment.DateCreated
    };
  }
}
