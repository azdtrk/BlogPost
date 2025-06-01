import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BlogPostService } from '../../../../core/services/blog-post.service';
import { BlogPost } from '../../../../core/models/blog-post.model';
import { CommonModule } from '@angular/common';
import { DisplayBlogPostListComponent } from '../display-blog-post-list/display-blog-post-list.component';

@Component({
  selector: 'app-author-blog-posts',
  templateUrl: './author-blog-posts.component.html',
  styleUrls: ['./author-blog-posts.component.scss'],
  standalone: true,
  imports: [CommonModule, DisplayBlogPostListComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AuthorBlogPostsComponent implements OnInit {
  authorId: string | null = null;
  authorName: string | null = null;
  blogPosts: BlogPost[] = [];
  isLoading = true;
  errorMessage: string | null = null;
  currentPage = 0;
  pageSize = 5;
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private blogPostService: BlogPostService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.authorId = params.get('id');
      this.authorName = params.get('name');
      
      if (this.authorId) {
        this.loadBlogPosts();
      } else {
        this.errorMessage = 'Author ID is required';
        this.isLoading = false;
      }
    });
  }

  loadBlogPosts(): void {
    if (!this.authorId) return;
    
    this.isLoading = true;
    this.errorMessage = null;
    
    this.blogPostService.getByAuthorId(this.authorId, this.currentPage, this.pageSize)
      .subscribe({
        next: (posts) => {
          this.blogPosts = posts;
          this.isLoading = false;
          
          // If we got the first blog post with author info, use it to set author name if needed
          if (posts.length > 0 && posts[0].author && !this.authorName) {
            this.authorName = posts[0].author.username;
          }
        },
        error: (error) => {
          this.errorMessage = `Error loading blog posts: ${error.message}`;
          this.isLoading = false;
        }
      });
  }
  
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadBlogPosts();
  }
} 