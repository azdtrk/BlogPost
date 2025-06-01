import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class AuthenticationComponent implements OnInit {
  authForm: FormGroup;
  isLoginMode = true;
  errorMessage: string = '';
  isLoading = false;
  returnUrl: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.authForm = this.createLoginForm();
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.redirectBasedOnRole();
      return;
    }

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    if (this.route.snapshot.queryParams['expired'] === 'true') {
      this.errorMessage = 'Your session has expired. Please login again.';
    }
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.authForm = this.isLoginMode ? this.createLoginForm() : this.createRegisterForm();
    this.errorMessage = '';
  }

  setRole(role: string): void {
    this.authForm.get('role')?.setValue(role);
  }

  onSubmit(): void {
    if (!this.authForm.valid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formData = this.authForm.value;

    if (this.isLoginMode) {
      const loginCredentials = {
        userNameOrEmail: formData.email,
        password: formData.password
      };

      this.authService.login(loginCredentials).subscribe({
        next: (response) => {
          this.isLoading = false;

          if (this.authService.validateAndRefreshTokenIfNeeded()) {
            if (this.returnUrl && this.returnUrl !== '/') {
              this.router.navigateByUrl(this.returnUrl);
            } else {
              this.authService.redirectToDashboard();
            }
          } else {
            this.errorMessage = 'Authentication succeeded but token validation failed. Please try again.';
          }
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.message || 'Authentication failed';
        }
      });
    } else {
      const registerData = {
        email: formData.email,
        username: formData.username || formData.email.split('@')[0],
        password: formData.password,
        role: formData.role
      };

      this.authService.register(registerData).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.authService.redirectToDashboard();
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.message || 'Registration failed';
        }
      });
    }
  }

  private redirectBasedOnRole(): void {
    const currentUser = this.authService.getCurrentUser();

    if (!currentUser) {
      this.errorMessage = 'Authentication successful but user data is missing';
      if (this.authService.isAuthenticated()) {
        this.authService.redirectToDashboard();
      }
      return;
    }

    this.authService.redirectToDashboard();
  }

  private createLoginForm(): FormGroup {
    return this.fb.group({
      email: ['azdtrk@gmail.com', [Validators.required, Validators.email]],
      password: ['Gt3ClubSport2135', [Validators.required, Validators.minLength(6)]]
    });
  }

  private createRegisterForm(): FormGroup {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      role: ['reader', Validators.required]
    });
  }
}
