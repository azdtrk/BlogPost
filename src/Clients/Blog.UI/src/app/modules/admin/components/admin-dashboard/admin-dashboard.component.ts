import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { BlogPostService } from '../../../../core/services/blog-post.service';

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
  searchTerm = '';

  constructor(
    private blogPostService: BlogPostService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadPosts();
  }

  filterPosts(): void {
    if (!this.searchTerm.trim()) {
      this.filteredPosts = [...this.posts];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredPosts = this.posts.filter(post => 
      post.title.toLowerCase().includes(searchTermLower) ||
      post.content.toLowerCase().includes(searchTermLower) ||
      post.author?.username.toLowerCase().includes(searchTermLower)
    );
  }

  viewPost(postId: number): void {
    this.router.navigate(['/post', postId]);
  }

  editPost(postId: number): void {
    this.router.navigate(['/admin/edit', postId]);
  }

  deletePost(postId: number): void {
    if (confirm('Are you sure you want to delete this post?')) {
      this.blogPostService.delete(postId).subscribe({
        next: () => {
          this.posts = this.posts.filter(post => post.id !== postId);
          this.filterPosts();
        },
        error: (error) => {
          console.error('Error deleting post:', error);
        }
      });
    }
  }

  private loadPosts(): void {
    this.isLoading = true;
    this.blogPostService.getAll().subscribe({
      next: (posts) => {
        this.posts = posts;
        this.filteredPosts = posts;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading posts:', error);
        this.isLoading = false;
      }
    });
  }
} 