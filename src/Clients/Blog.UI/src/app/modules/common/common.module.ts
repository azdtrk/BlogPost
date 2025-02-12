import { NgModule } from '@angular/core';
import { CommonModule as NgCommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { DisplayBlogPostListComponent } from './components/display-blog-post-list/display-blog-post-list.component';
import { DisplayBlogPostComponent } from './components/display-blog-post/display-blog-post.component';

@NgModule({
  declarations: [
    
  ],
  imports: [
    DisplayBlogPostListComponent,
    DisplayBlogPostComponent,
    NgCommonModule,
    RouterModule,
    ReactiveFormsModule
  ],
  exports: [
    DisplayBlogPostListComponent,
    DisplayBlogPostComponent
  ]
})
export class CommonModule { } 