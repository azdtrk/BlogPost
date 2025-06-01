import { Routes } from '@angular/router';
import { DisplayBlogPostComponent } from './modules/common/components/display-blog-post/display-blog-post.component';
import { AuthenticationComponent } from './modules/authentication/components/authentication/authentication.component';
import { UserDashboardComponent } from './modules/user/components/user-dashboard/user-dashboard.component';
import { BlogPageComponent } from './modules/common/components/blog-page/blog-page.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthRedirectGuard } from './core/guards/auth-redirect.guard';
import { AuthorBlogPostsComponent } from './modules/common/components/author-blog-posts/author-blog-posts.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/blog',
    pathMatch: 'full'
  },
  {
    path: 'blog',
    component: BlogPageComponent
  },
  {
    path: 'post/:id',
    component: DisplayBlogPostComponent
  },
  {
    path: 'post-by-title/:title',
    component: DisplayBlogPostComponent
  },
  {
    path: 'author/:id',
    component: AuthorBlogPostsComponent
  },
  {
    path: 'author/:id/:name',
    component: AuthorBlogPostsComponent
  },
  {
    path: 'login',
    component: AuthenticationComponent,
    canActivate: [AuthRedirectGuard]
  },
  {
    path: 'user',
    component: UserDashboardComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard, AdminGuard],
    data: { roles: ['author'] }
  },
  {
    path: '**',
    redirectTo: '/blog'
  }
]; 