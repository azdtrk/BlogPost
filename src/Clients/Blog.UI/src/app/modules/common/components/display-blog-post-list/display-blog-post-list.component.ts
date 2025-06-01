import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { CommentService } from '../../../../core/services/comment.service';
import { AuthService } from '../../../../core/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { environment } from '../../../../environments/environment';
import { finalize } from 'rxjs/operators';
import {Comment} from '../../../../core/models/comment.model';
import { of, throwError } from 'rxjs';

@Component({
  selector: 'app-display-blog-post-list',
  templateUrl: './display-blog-post-list.component.html',
  styleUrls: ['./display-blog-post-list.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe]
})
export class DisplayBlogPostListComponent implements OnInit {
  @Input() blogPosts: BlogPost[] = [];
  @Output() pageChange = new EventEmitter<number>();
  
  commentForm: FormGroup;
  selectedPostId: number | null = null;
  isLoading = false;
  error: string | null = null;
  currentPage = 0;
  pageSize = 4;
  defaultThumbnailPath = 'assets/default-thumbnail.jpeg';
  isEmpty = false;
  hasError = false;
  errorMessage: string | null = null;

  constructor(
    private blogPostService: BlogPostService,
    private commentService: CommentService,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.commentForm = this.fb.group({
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    // Only load blog posts if none were provided through input
    if (this.blogPosts.length === 0) {
      this.loadBlogPosts();
    }
  }

  loadBlogPosts(): void {
    this.isLoading = true;
    this.error = null;
    this.hasError = false;

    this.blogPostService.getAll(this.currentPage, this.pageSize)
      .pipe(
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: (response: any) => {
          // Handle different response formats
          let postsArray: any[] = [];

          if (Array.isArray(response)) {
            postsArray = response;
          } else if (response && typeof response === 'object') {
            const properties = ['Value', 'value', 'data', 'items', 'blogPosts', 'posts', 'content'];
            for (const prop of properties) {
              if (response[prop] && Array.isArray(response[prop])) {
                postsArray = response[prop];
                break;
              }
            }
          }

          if (postsArray && postsArray.length > 0) {
            try {
              this.blogPosts = postsArray.map(post => this.normalizePost(post));
              this.isEmpty = false;
            } catch (error) {
              this.hasError = true;
              this.errorMessage = 'Error processing blog posts data';
              this.blogPosts = [];
              this.isEmpty = true;
            }
          } else {
            this.blogPosts = [];
            this.isEmpty = true;
          }
        },
        error: (error) => {
          this.hasError = true;
          this.errorMessage = 'Failed to load blog posts. Please try again later.';
          this.blogPosts = [];
          this.isEmpty = true;
        }
      });
  }

  navigateToBlogPost(postId: string | undefined): void {
    // Find the post that was clicked
    const clickedPost = this.blogPosts.find(p => p.id === postId);

    if (clickedPost) {
      this.navigateByTitleAndId(clickedPost);
      return;
    }

    if (this.blogPosts.length > 0) {
      this.navigateByTitleAndId(this.blogPosts[0]);
    } else {
      alert('No blog posts available to navigate to.');
    }
  }

  navigateByTitleAndId(post: BlogPost): void {
    if (!post) {
      return;
    }

    try {
      // GUID/UUID regex pattern
      const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

      // Validate post ID
      if (post.id && guidPattern.test(post.id)) {
        // Pass the entire post object to avoid loading issues
        this.router.navigate(['/post', post.id], {
          state: { postData: post }
        });
      } else if (post.title) {
        const titleSlug = post.title
          .toLowerCase()
          .replace(/[^\w\s-]/g, '') // Remove special characters
          .replace(/\s+/g, '-')     // Replace spaces with hyphens
          .trim();                  // Trim leading/trailing spaces

        this.router.navigate(['/post-by-title', titleSlug], {
          state: { postData: post } // Pass the entire post object
        });
      } else {
        alert('Cannot display this blog post. Missing identifier.');
      }
    } catch (error) {
      alert('Error navigating to blog post. See console for details.');
    }
  }

  loadNextPage(): void {
    this.currentPage++;
    if (this.pageChange.observed) {
      this.pageChange.emit(this.currentPage);
    } else {
      this.loadBlogPosts();
    }
  }

  loadPreviousPage(): void {
    if (this.currentPage > 0) {
      this.currentPage--;
      if (this.pageChange.observed) {
        this.pageChange.emit(this.currentPage);
      } else {
        this.loadBlogPosts();
      }
    }
  }

  handleImageError(event: Event): void {
    // Replace broken image with default
    const imgElement = event.target as HTMLImageElement;
    imgElement.src = this.defaultThumbnailPath;
    imgElement.classList.add('fallback-image');
  }

  handleImageLoad(event: Event): void {
    // Image loaded successfully
    const imgElement = event.target as HTMLImageElement;
    imgElement.classList.add('loaded');
  }

  private normalizePost(post: any): BlogPost {
    // Map API fields to expected model properties
    // The API returns fields with capital first letters (Title, Preface, etc.)
    return {
      id: post.Id || post.id || '',
      title: post.Title || post.title || 'Untitled Post',
      preface: post.Preface || post.preface || 'No description available',
      content: post.Content || post.content || '',
      dateCreated: post.DateCreated || post.dateCreated || null,
      author: post.Author || post.author || { id: '', username: 'Unknown Author' },
      thumbNailImage: post.ThumbNailImage || post.thumbNailImage || null
    };
  }
}
