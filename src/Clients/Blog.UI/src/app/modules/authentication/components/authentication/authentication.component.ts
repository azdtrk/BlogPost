import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AuthenticationComponent {
  authForm: FormGroup;
  isLoginMode = true;
  errorMessage: string = '';
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.authForm = this.createLoginForm();
  }

  toggleMode(): void {
    this.isLoginMode = !this.isLoginMode;
    this.authForm = this.isLoginMode ? this.createLoginForm() : this.createRegisterForm();
    this.errorMessage = '';
  }

  onSubmit(): void {
    if (!this.authForm.valid) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    const credentials = this.authForm.value;
    
    const authObservable = this.isLoginMode ? 
      this.authService.login(credentials) : 
      this.authService.register(credentials);

    authObservable.subscribe({
      next: (response) => {
        this.isLoading = false;
        if (this.authService.hasRole('admin')) {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/user']);
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.message || 'Authentication failed';
        console.error('Authentication failed:', error);
      }
    });
  }
  
  private createLoginForm(): FormGroup {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  private createRegisterForm(): FormGroup {
    return this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      firstName: [''],
      lastName: ['']
    });
  }
} 