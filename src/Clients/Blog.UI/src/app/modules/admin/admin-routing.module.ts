import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { CreateBlogPostComponent } from './components/create-blog-post/create-blog-post.component';
import { ManageCommentsComponent } from './components/manage-comments/manage-comments.component';

const routes: Routes = [
  { path: '', component: AdminDashboardComponent },
  { path: 'create', component: CreateBlogPostComponent },
  { path: 'comments', component: ManageCommentsComponent },
  { path: 'edit/:id', component: CreateBlogPostComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { } 