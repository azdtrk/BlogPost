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
  isLoading = false;

  constructor(
    private router: Router
  ) {}

  ngOnInit(): void {

  }

  viewPost(postId: number): void {
    this.router.navigate(['/post', postId]);
  }
}
