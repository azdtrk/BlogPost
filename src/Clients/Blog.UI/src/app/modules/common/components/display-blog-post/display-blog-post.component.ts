import { Component, Input, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { Comment } from '../../../../core/models/comment.model';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { CommentService } from '../../../../core/services/comment.service';
import { AuthService } from '../../../../core/services/auth.service';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-display-blog-post',
  templateUrl: './display-blog-post.component.html',
  styleUrls: ['./display-blog-post.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe, RouterModule]
})
export class DisplayBlogPostComponent implements OnInit {
  @Input() postId?: string;
  post?: BlogPost;
  comments: Comment[] = [];
  commentForm: FormGroup;
  isAuthenticated: boolean = false;
  isLoading: boolean = true;
  error?: string;
  titleSlug?: string;
  debugInfo: boolean = true;
  apiUrl?: string;
  requestTime?: Date;

  constructor(
    private blogPostService: BlogPostService,
    private commentService: CommentService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.commentForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(3)]]
    });

  }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.isLoading = true;

    const navigation = this.router.getCurrentNavigation();
    if (navigation && navigation.extras.state) {
      const state = navigation.extras.state as { postData: BlogPost };
      if (state.postData) {
        this.post = state.postData;
        this.postId = this.post.id;
        this.loadComments();
        this.isLoading = false;
        return;
      }
    }

    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.postId = idParam; // Use string ID directly
        this.loadData();
        return;
      }

      const titleParam = params.get('title');
      if (titleParam) {
        this.titleSlug = titleParam;
        this.loadPostByTitle(titleParam);
        return;
      }

      if (this.postId) {
        this.loadData();
        return;
      }

      this.error = 'Blog post not found - no identifier provided';
      this.isLoading = false;
    });
  }

  loadData(): void {
    this.isLoading = true;
    this.loadPost();
    this.loadComments();
}

  submitComment(): void {

    if (this.commentForm.valid && this.postId) {
      const comment: Partial<Comment> = {
        content: this.commentForm.value.content,
        blogPostId: this.postId
      };

      this.commentService.create(comment).subscribe({
        next: (newComment: Comment) => {
          this.comments.unshift(newComment);
          this.commentForm.reset();
        }
      });

    }
  }

  private loadPost(): void {
    if (!this.postId) {
      this.error = 'Post ID is required';
      this.isLoading = false;
      return;
    }

    const guidPattern = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

    if (!guidPattern.test(this.postId) && environment.production) {
      this.error = `Invalid post ID format. Expected a valid GUID.`;
      this.isLoading = false;
      return;
    }

    this.requestTime = new Date();

    this.apiUrl = `/api/BlogPost/${this.postId}`;
    this.blogPostService.getById(this.postId)
      .pipe(
        catchError(error => {
          this.error = error.message || 'Failed to load blog post';
          this.isLoading = false;
          return of(null); // Return null to continue the stream without error
        })
      )
      .subscribe({
        next: (post: any) => {
          if (!post) {
            return; // Error was handled in catchError
          }

          if (post && typeof post === 'object') {
            if (!post.id && !post.title) {
              if (post.value && typeof post.value === 'object') {
                post = post.value;
              } else if (post.data && typeof post.data === 'object') {
                post = post.data;
              } else if (post.item && typeof post.item === 'object') {
                post = post.item;
              }
            }
          }

          if (!post || !post.title) {
            this.error = 'Blog post data is invalid or incomplete';
            this.isLoading = false;
            return;
          }

          this.post = post;
          this.isLoading = false;
        },
        error: (error) => {
          this.error = 'Failed to load blog post: ' + (error.message || 'Unknown error');
          this.isLoading = false;
        }
      });
  }

  private loadComments(): void {
    if (this.postId) {
      this.commentService.getById(this.postId).subscribe({
        next: (response) => {
          if (Array.isArray(response)) {
            this.comments = response;
          } else if (response) {
            this.comments = [response];
          } else {
            this.comments = [];
          }
        },
        error: (error) => {
          this.comments = [];
        }
      });
    } else {
      this.comments = [];
    }
  }

  private loadPostByTitle(titleSlug: string): void {
    if (!titleSlug) {
      this.error = 'Title slug is required';
      this.isLoading = false;
      return;
    }

    this.requestTime = new Date();

    this.apiUrl = `/api/BlogPost?title=${encodeURIComponent(titleSlug)}`;

    this.isLoading = true;

    this.blogPostService.getAll(0, 100)
      .pipe(
        catchError(error => {
          this.error = error.message || 'Failed to load blog posts';
          this.isLoading = false;
          return of([]); // Return empty array to continue the stream without error
        })
      )
      .subscribe({
        next: (posts: any) => {
          let postsArray: any[] = [];

          if (Array.isArray(posts)) {
            postsArray = posts;
          } else if (posts && typeof posts === 'object') {
            const possibleArrayProps = ['value', 'Value', 'data', 'items', 'blogPosts', 'posts', 'content'];
            for (const prop of possibleArrayProps) {
              if (posts[prop] && Array.isArray(posts[prop])) {
                postsArray = posts[prop];
                break;
              }
            }
          }

          if (!postsArray.length) {
            this.error = 'No blog posts found';
            this.isLoading = false;
            return;
          }

          // Find post with matching title slug
          const foundPost = postsArray.find(post => {
            if (!post.title) return false;

            const postSlug = post.title
              .toLowerCase()
              .replace(/[^\w\s-]/g, '')
              .replace(/\s+/g, '-')
              .trim();

            return postSlug === titleSlug;
          });

          if (foundPost) {
            this.post = foundPost;
            this.postId = foundPost.id;
            this.loadComments();
          } else {
            this.error = `Blog post with title "${titleSlug}" not found`;
          }

          this.isLoading = false;
        },
        error: (error) => {
          this.error = 'Failed to load blog posts: ' + (error.message || 'Unknown error');
          this.isLoading = false;
        }
      });
  }

  retryLoading(): void {
    this.error = undefined;
    this.isLoading = true;
    this.requestTime = new Date(); // Reset the request time for the retry

    if (this.postId) {
      this.loadData();
    } else if (this.titleSlug) {
      this.loadPostByTitle(this.titleSlug);
    } else {
      this.error = 'Cannot retry: No post identifier available';
      this.isLoading = false;
    }
  }
}
