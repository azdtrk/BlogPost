import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthRedirectGuard } from './core/guards/auth-redirect.guard';
import { PostIdValidatorGuard } from './core/guards/post-id-validator.guard';
import { DisplayBlogPostListComponent } from './modules/common/components/display-blog-post-list/display-blog-post-list.component';
import { DisplayBlogPostComponent } from './modules/common/components/display-blog-post/display-blog-post.component';
import { AuthenticationComponent } from './modules/authentication/components/authentication/authentication.component';
import { UserDashboardComponent } from './modules/user/components/user-dashboard/user-dashboard.component';
import { BlogPageComponent } from './modules/common/components/blog-page/blog-page.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: 'blog',
    component: BlogPageComponent
  },
  {
    path: 'post/:id',
    component: DisplayBlogPostComponent,
    canActivate: [PostIdValidatorGuard]
  },
  {
    path: 'post-by-title/:title',
    component: DisplayBlogPostComponent
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
    redirectTo: '/login'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: true,
    onSameUrlNavigation: 'reload'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
