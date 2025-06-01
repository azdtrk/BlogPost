# Frontend Repository Pattern Implementation

This directory contains the implementation of the repository pattern for the frontend part of the Blog application.

## Structure

The services are organized in a hierarchical manner:

1. **HttpService**: The lowest-level service that handles HTTP communication with the backend
2. **BaseService<T>**: Generic base service that implements CRUD operations for entities
3. **AuthBaseService<T>**: Specialized base service for authentication
4. **Entity-specific services**: Services for specific entities that extend BaseService or AuthBaseService

## Services Overview

### HttpService

The `HttpService` is a wrapper around Angular's HttpClient that standardizes HTTP requests. It:

- Manages the base API URL
- Provides methods for common HTTP operations (GET, POST, PUT, DELETE)
- Handles common headers and request configuration

### BaseService<T>

The `BaseService<T>` is a generic service that implements CRUD operations for entities. It:

- Defines an abstract `apiPath` property that entity-specific services must implement
- Provides methods for CRUD operations (getAll, getById, create, update, delete)
- Normalizes API responses to handle different response formats
- Includes error handling

### AuthBaseService<T>

The `AuthBaseService<T>` is a specialized service for authentication. It:

- Defines abstract properties for API path and token storage
- Provides methods for login, register, and token refresh
- Includes token management (saving, retrieving, removing)
- Defines abstract methods for authentication state (isAuthenticated, getCurrentUser)

### Entity-specific Services

Entity-specific services extend the base services and implement entity-specific logic:

- **BlogPostService**: Handles blog post operations
- **CommentService**: Handles comment operations
- **AuthService**: Handles authentication operations

## Usage Example

```typescript
// Creating a new blog post
this.blogPostService.create(newBlogPost).subscribe({
  next: (createdPost) => {
    console.log('Blog post created:', createdPost);
  },
  error: (error) => {
    console.error('Failed to create blog post:', error);
  }
});

// Getting all comments
this.commentService.getAll().subscribe({
  next: (comments) => {
    this.comments = comments;
  },
  error: (error) => {
    console.error('Failed to fetch comments:', error);
  }
});

// Authenticating a user
this.authService.login(credentials).subscribe({
  next: () => {
    this.authService.redirectToDashboard();
  },
  error: (error) => {
    this.errorMessage = error.message;
  }
});
```

## Benefits

This repository pattern implementation provides several benefits:

1. **Code Reuse**: Common functionality is implemented in base services and reused
2. **Consistency**: All services follow the same pattern for API communication
3. **Type Safety**: Generic typing ensures type safety throughout the application
4. **Abstraction**: Services abstract away the details of API communication
5. **Maintainability**: Changes to API endpoints or authentication can be made in one place
6. **Testability**: Services can be easily mocked for testing 