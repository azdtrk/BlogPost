import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { CommentService } from '../../../../core/services/comment.service';
import { AuthService } from '../../../../core/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { BlogPost } from '../../../../core/models/blog-post.model';

@Component({
  selector: 'app-display-blog-post-list',
  templateUrl: './display-blog-post-list.component.html',
  styleUrls: ['./display-blog-post-list.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DatePipe]
})
export class DisplayBlogPostListComponent implements OnInit {
  blogPosts: BlogPost[] = [];
  commentForm: FormGroup;
  selectedPostId: number | null = null;
  isLoading = false;
  error: string | null = null;

  constructor(
    private blogPostService: BlogPostService,
    private commentService: CommentService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.commentForm = this.fb.group({
      content: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadBlogPosts();
  }

  loadBlogPosts(): void {
    this.isLoading = true;
    this.error = null;
    
    this.blogPostService.getAll().subscribe({
      next: (posts) => {
        this.blogPosts = posts;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading blog posts:', error);
        this.error = 'Failed to load blog posts. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  submitComment(postId: number): void {
    if (this.commentForm.valid) {
      const commentData = {
        content: this.commentForm.get('content')?.value,
        blogPostId: postId
      };

      this.commentService.create(commentData).subscribe({
        next: () => {
          this.commentForm.reset();
          this.loadBlogPosts(); // Reload posts to show new comment
        },
        error: (error) => {
          console.error('Error submitting comment:', error);
        }
      });
    }
  }

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }
} 