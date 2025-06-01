import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DisplayBlogPostListComponent } from '../display-blog-post-list/display-blog-post-list.component';

@Component({
  selector: 'app-blog-page',
  templateUrl: './blog-page.component.html',
  styleUrls: ['./blog-page.component.scss'],
  standalone: true,
  imports: [CommonModule, DisplayBlogPostListComponent]
})
export class BlogPageComponent {
  constructor() {}
} 