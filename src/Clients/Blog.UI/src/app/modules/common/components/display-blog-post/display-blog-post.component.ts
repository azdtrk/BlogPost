import { Component, Input, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { Comment } from '../../../../core/models/comment.model';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { CommentService } from '../../../../core/services/comment.service';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-display-blog-post',
  templateUrl: './display-blog-post.component.html',
  styleUrls: ['./display-blog-post.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class DisplayBlogPostComponent implements OnInit {
  @Input() postId?: number;
  post?: BlogPost;
  comments: Comment[] = [];
  commentForm: FormGroup;
  isAuthenticated: boolean = false;

  constructor(
    private blogPostService: BlogPostService,
    private commentService: CommentService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.commentForm = this.fb.group({
      content: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    if (this.postId) {
      this.loadPost();
      this.loadComments();
    }
  }

  submitComment(): void {
    if (this.commentForm.valid && this.postId) {
      const comment = {
        content: this.commentForm.value.content,
        blogPostId: this.postId
      };

      this.commentService.create(comment).subscribe({
        next: (newComment) => {
          this.comments.unshift(newComment);
          this.commentForm.reset();
        },
        error: (error) => {
          console.error('Error submitting comment:', error);
        }
      });
    }
  }

  private loadPost(): void {
    if (this.postId) {
      this.blogPostService.getById(this.postId).subscribe({
        next: (post) => this.post = post,
        error: (error) => console.error('Error loading post:', error)
      });
    }
  }

  private loadComments(): void {
    if (this.postId) {
      this.commentService.getByPostId(this.postId).subscribe({
        next: (comments) => this.comments = comments,
        error: (error) => console.error('Error loading comments:', error)
      });
    }
  }
} 