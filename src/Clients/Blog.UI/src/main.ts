import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { bootstrapApplication } from '@angular/platform-browser';
import '@angular/compiler';
import { AppModule } from './app/app.module';
import { AppComponent } from './app/app.component';
import { appConfig } from './app/app.config';
import { environment } from './app/environments/environment';

// Try the standalone approach first
bootstrapApplication(AppComponent, appConfig)
  .catch(err => {
    // Fall back to NgModule approach
    platformBrowserDynamic().bootstrapModule(AppModule, {
      preserveWhitespaces: false
    })
    .catch(moduleErr => console.error('Error bootstrapping the application:', moduleErr));
  });
