import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, NavigationEnd, ActivatedRoute, RouterOutlet } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { AuthService } from './core/services/auth.service';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [NavbarComponent, RouterOutlet, CommonModule],
  standalone: true
})
export class AppComponent implements OnInit {
  isLoginPage: boolean = false;

  constructor(
    private titleService: Title,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.refreshAuthState();
    this.checkAuthState();

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute;
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      }),
      filter(route => route.outlet === 'primary')
    ).subscribe(route => {
      const routeData = route.snapshot.data;
      let title = 'My Own Blog';
      
      // Update isLoginPage based on current route
      this.isLoginPage = this.router.url.includes('/login');

      switch (this.router.url.split('?')[0]) {
        case '/login':
          title = 'My Own Blog | Login';
          break;
        case '/admin':
          title = 'My Own Blog | Admin Dashboard';
          break;
        case '/user':
          title = 'My Own Blog | User Dashboard';
          break;
        case '/blog':
          title = 'My Own Blog | Articles';
          break;
        default:
          if (routeData['title']) {
            title = `My Own Blog | ${routeData['title']}`;
          }
      }
      this.titleService.setTitle(title);
    });
  }

  refreshAuthState(): void {
    if (!this.authService.isAuthenticated()) {
      // Token is either expired or invalid
      if (this.authService.getToken()) {
        // If token exists but isAuthenticated() returns false, it must be expired
        this.authService.logout();
        this.router.navigate(['/login'], { queryParams: { expired: 'true' } });
      }
    }
  }

  private checkAuthState(): void {
    if (!this.authService.isAuthenticated() && this.router.url === '/login') {
      this.isLoginPage = true;
      return;
    }

    if (this.authService.isAuthenticated()) {
      const currentUrl = this.router.url;

      if (currentUrl === '/' || currentUrl === '/login' || currentUrl === '/register') {
        this.authService.redirectToDashboard();
      } else if (currentUrl.startsWith('/admin') && !this.authService.isAdmin()) {
        this.router.navigate(['/user']);
      } else if (currentUrl.startsWith('/user') && this.authService.isAdmin()) {
        this.router.navigate(['/admin']);
      }
    }
  }
}
