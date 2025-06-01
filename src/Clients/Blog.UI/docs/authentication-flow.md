# Authentication Flow Documentation

## Overview

This document explains the authentication process in the blog application, including login flow, registration flow, and role-based routing.

## Application Initial Load

When the application first loads:
1. The default route redirects to the `/login` page
2. If the user is already authenticated (has a valid token in localStorage):
   - The `AuthRedirectGuard` will automatically redirect them to the appropriate dashboard:
     - Admin/Author users → Admin Dashboard
     - Reader users → User Dashboard
3. If not authenticated, the login form is displayed

## User Roles

The application supports two main user roles:

1. **Author/Admin** - Users with administrative privileges who can create and manage blog content
2. **Reader** - Regular users who can read blog posts and submit comments

## Authentication Flow

### Login Process

1. User submits login credentials (email and password) via the login form
2. Authentication service sends credentials to the API endpoint `/api/auth/login`
3. After successful authentication:
   - JWT token is stored in local storage
   - User information is extracted from the JWT token
   - User is redirected based on their role:
     - Author/Admin users → Admin dashboard (`/admin`)
     - Reader users → User dashboard (`/user`)

### Registration Process

1. User submits registration information (username, email, password, optional name)
2. Authentication service sends data to the API endpoint `/api/auth/register`
3. After successful registration:
   - New users are assigned the Reader role by default
   - JWT token is stored in local storage
   - User is automatically logged in and redirected to the User dashboard

## Route Protection

Routes are protected using Angular Guards:

- `AuthGuard` - Verifies user is authenticated, redirects to login if not
- `AdminGuard` - Checks if authenticated user has admin role, redirects to user dashboard if not
- `AuthRedirectGuard` - Redirects authenticated users away from the login page to their appropriate dashboard

## Token Management

- JWT tokens are automatically attached to API requests through the `JwtInterceptor`
- The interceptor handles:
  - Adding Authorization headers to requests
  - Handling 401 Unauthorized errors (redirecting to login)
  - Handling 403 Forbidden errors (redirecting based on role)

## Security Features

1. Passwords are never stored on the client side
2. JWT tokens are validated for expiration before each API request
3. Sensitive routes are protected by appropriate guards
4. API endpoints that require authorization are documented

## Implementation Details

The authentication system consists of several key components:

- `AuthService` - Core service handling login, registration, and token management
- `JwtInterceptor` - HTTP interceptor for managing authorization headers
- `AuthGuard`, `AdminGuard`, and `AuthRedirectGuard` - Route protection
- `AuthenticationComponent` - UI component for login and registration

## Session Management

- User sessions are maintained via JWT tokens
- Tokens have a limited lifetime (typically 2 hours)
- If a token expires, the user is automatically redirected to the login page
- Sessions can be manually terminated by using the logout function 