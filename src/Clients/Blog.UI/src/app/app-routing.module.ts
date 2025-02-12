import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { DisplayBlogPostListComponent } from './modules/common/components/display-blog-post-list/display-blog-post-list.component';
import { AuthenticationComponent } from './modules/authentication/components/authentication/authentication.component';
import { UserDashboardComponent } from './modules/user/components/user-dashboard/user-dashboard.component';
import { AdminDashboardComponent } from './modules/admin/components/admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  { 
    path: '', 
    redirectTo: '/blog', 
    pathMatch: 'full' 
  },
  { 
    path: 'blog', 
    component: DisplayBlogPostListComponent 
  },
  { 
    path: 'login', 
    component: AuthenticationComponent 
  },
  { 
    path: 'user', 
    component: UserDashboardComponent,
    canActivate: [AuthGuard]
  },
  { 
    path: 'admin', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard, AdminGuard] 
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { } 