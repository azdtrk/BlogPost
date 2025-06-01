import { Injectable } from '@angular/core';
import { HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { BaseService } from './base.service';
import { HttpService } from './http/http.service';
import { map, catchError } from 'rxjs/operators';

export interface ImageResponse {
  id: string;
  fileName: string;
  path: string;
  storage: string;
  blogPostId?: string;
  thumbnailForBlogPostId?: string;
}

@Injectable({
  providedIn: 'root'
})
export class BlogPostService extends BaseService<BlogPost> {
  protected override apiPath = '/api/BlogPost';
  public apiUrl = '/api/BlogPost';

  constructor(httpService: HttpService) {
    super(httpService);
  }

  override getAll(page: number = 0, size: number = 5, params?: HttpParams): Observable<BlogPost[]> {
    return super.getAll(page, size, params);
  }

  /**
   * Get blog posts by author ID
   * @param authorId The ID of the author to fetch posts for
   * @param page Page number for pagination
   * @param size Page size for pagination
   */
  getByAuthorId(authorId: string, page: number = 0, size: number = 10): Observable<BlogPost[]> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('size', size.toString());

    return this.httpService.get<any>(`${this.apiPath}/author/${authorId}`, params).pipe(
      map(response => this.normalizeResponse(response)),
      catchError(error => this.handleError(`Failed to fetch blog posts for author ${authorId}`, error))
    );
  }

  /**
   * Creates a draft blog post in the database
   * @param authorId The author's ID
   * @returns Observable of the created draft blog post
   */
  createDraft(authorId: string): Observable<BlogPost> {
    const draftData = {
      title: 'Draft Post',
      preface: 'Draft preface...',
      content: '',
      authorId: authorId,
      canBePublished: false
    };

    return this.httpService.post<any>(`${this.apiPath}`, draftData).pipe(
      map(response => {
        // Normalize the response to extract the ID of the created draft
        if (response && (response.id || response.Id)) {
          return {
            id: response.id || response.Id,
            title: draftData.title,
            preface: draftData.preface,
            content: draftData.content
          };
        } else if (response && response.value) {
          // Handle nested response structure if needed
          return { id: response.value, ...draftData };
        }
        return draftData;
      }),
      catchError(error => this.handleError('Failed to create draft blog post', error))
    );
  }

  uploadImage(formData: FormData, blogPostId?: string, thumbnailForBlogPostId?: string): Observable<ImageResponse> {
    // Add the IDs to the form data if they're provided
    if (blogPostId) {
      formData.append('BlogPostId', blogPostId);
    }
    
    if (thumbnailForBlogPostId) {
      formData.append('ThumbnailForBlogPostId', thumbnailForBlogPostId);
    }

    return new Observable<ImageResponse>(observer => {
      // Use direct fetch to bypass Angular HTTP client and interceptors
      const uploadUrl = 'https://localhost:7165/api/BlogPost/upload-image';
      console.log('Uploading image to:', uploadUrl, formData);
      
      // Log the form data contents for debugging
      console.log('Form data contents:');
      for (const pair of (formData as any).entries()) {
        console.log(`${pair[0]}: ${pair[1]}`);
      }
      
      fetch(uploadUrl, {
        method: 'POST',
        body: formData
      })
      .then(response => {
        console.log('Upload response status:', response.status, response.statusText);
        if (!response.ok) {
          return response.text().then(text => {
            console.error('Upload error response:', text);
            throw new Error(`HTTP error ${response.status}: ${response.statusText}\nResponse: ${text}`);
          });
        }
        return response.json();
      })
      .then(data => {
        console.log('Upload successful, raw response:', data);
        let value = data;
        
        if (data.value) {
          value = data.value;
        } else if (data.Value) {
          value = data.Value;
        }
        
        if (!value || (!value.id && !value.Id)) {
          console.error('Invalid response format:', data);
          throw new Error('Server returned invalid response format');
        }
        
        const imageResponse: ImageResponse = {
          id: value.id || value.Id,
          fileName: value.fileName || value.FileName,
          path: value.path || value.Path,
          storage: value.storage || value.Storage || 'local',
          blogPostId: value.blogPostId || value.BlogPostId,
          thumbnailForBlogPostId: value.thumbnailForBlogPostId || value.ThumbnailForBlogPostId
        };
        
        console.log('Normalized image response:', imageResponse);
        observer.next(imageResponse);
        observer.complete();
      })
      .catch(error => {
        console.error('Upload error caught:', error);
        observer.error(error);
      });
    });
  }

  protected override normalizeResponse(response: any, isSingleItem: boolean = false): any {
    const normalizedData = super.normalizeResponse(response, isSingleItem);

    if (!normalizedData) {
      return normalizedData;
    }

    if (isSingleItem) {
      return this.mapToBlogPost(normalizedData);
    }

    return normalizedData.map((post: any) => this.mapToBlogPost(post));
  }

  private mapToBlogPost(post: any): BlogPost {
    return {
      id: post.id || post.Id,
      title: post.title || post.Title,
      content: post.content || post.Content,
      preface: post.preface || post.Preface,
      dateCreated: post.dateCreated || post.DateCreated,
      author: post.author || post.Author,
      images: post.images || post.Images,
      thumbNailImage: post.thumbNailImage || post.ThumbNailImage
    };
  }
}
