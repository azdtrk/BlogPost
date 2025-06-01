import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { AuthService } from '../../../../core/services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule]
})
export class AdminDashboardComponent implements OnInit {
  posts: BlogPost[] = [];
  filteredPosts: BlogPost[] = [];
  isLoading = false;
  searchTerm: string = '';
  isAdmin = false;
  isAuthenticated = false;
  userRole = 'unknown';
  errorMessage = '';

  constructor(
    private blogPostService: BlogPostService,
    private router: Router,
    private authService: AuthService,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isAuthenticated = this.authService.isAuthenticated();
    const user = this.authService.getCurrentUser();
    this.userRole = user?.role || 'none';
    this.loadPosts();
  }

  loadPosts(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.blogPostService.getAll(0, 8).subscribe({
      next: (posts) => {
        if (Array.isArray(posts)) {
          this.posts = posts;
          this.filteredPosts = posts;
        } else {
          this.posts = [];
          this.filteredPosts = [];
          this.errorMessage = 'Received an unexpected response format from the server';
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load blog posts. The API server might be unavailable or there might be database migration issues.';
        this.posts = [];
        this.filteredPosts = [];
      }
    });
  }

  viewPost(post: BlogPost): void {
    if (!post || !post.id) {
      return;
    }
    this.router.navigate(['/post', post.id]);
  }

  editPost(post: BlogPost): void {
    if (!post || !post.id) {
      return;
    }
    this.router.navigate(['/admin/edit', post.id]);
  }

  deletePost(post: BlogPost): void {
    if (!post || !post.id) {
      return;
    }

    if (confirm('Are you sure you want to delete this post?')) {
      this.blogPostService.delete(post.id).subscribe({
        next: () => {
          this.posts = this.posts.filter(p => p.id !== post.id);
          this.filterPosts();
        },
        error: (error) => {
          alert('Failed to delete post. Please try again.');
        }
      });
    }
  }

  filterPosts(): void {
    if (!this.searchTerm) {
      this.filteredPosts = [...this.posts];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredPosts = this.posts.filter(post => {
      if (!post) return false;

      return (
        post.title?.toLowerCase().includes(searchTermLower) ||
        post.content?.toLowerCase().includes(searchTermLower) ||
        post.preface?.toLowerCase().includes(searchTermLower)
      );
    });
  }

  createNewPost(): void {
    this.router.navigate(['/admin/create']);
  }

}
