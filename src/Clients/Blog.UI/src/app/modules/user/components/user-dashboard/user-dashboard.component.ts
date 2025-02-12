import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { CommonModule } from '@angular/common';
import { DisplayBlogPostListComponent } from '../../../common/components/display-blog-post-list/display-blog-post-list.component';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, DisplayBlogPostListComponent]
})
export class UserDashboardComponent implements OnInit {
  recentPosts: BlogPost[] = [];
  isLoading = false;

  constructor(
    private blogPostService: BlogPostService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadRecentPosts();
  }

  viewPost(postId: number): void {
    this.router.navigate(['/post', postId]);
  }

  private loadRecentPosts(): void {
    this.isLoading = true;
    this.blogPostService.getAll()
      .subscribe({
        next: (posts) => {
          this.recentPosts = posts;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading posts:', error);
          this.isLoading = false;
        }
      });
  }
} 