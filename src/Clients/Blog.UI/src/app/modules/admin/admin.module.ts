import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { CreateBlogPostComponent } from './components/create-blog-post/create-blog-post.component';
import { ManageCommentsComponent } from './components/manage-comments/manage-comments.component';
import { AdminRoutingModule } from './admin-routing.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    AdminRoutingModule,
    AdminDashboardComponent,
    CreateBlogPostComponent,
    ManageCommentsComponent
  ]
})
export class AdminModule { } 