# Blog Application Data Persistence Layer

## Authentication and Authorization

### Admin User

The application comes with a pre-configured admin user. The admin user is automatically created when the application starts if it doesn't already exist.

- Username: `admin`
- Email: `azdtrk@gmail.com`
- Password: Stored in user secrets

The admin password is stored in user secrets for security reasons and not hardcoded in the source code.

### Password Security

All passwords in the application are securely stored using ASP.NET Core Identity's password hashing. Passwords are:

1. Never stored in plain text - only password hashes are stored
2. Hashed using strong cryptographic algorithms (PBKDF2 with HMAC-SHA256)
3. Protected with a unique salt for each user to prevent rainbow table attacks
4. The plaintext Password field has been removed from the User entity in favor of only storing PasswordHash

Even administrators cannot see users' passwords as they are one-way hashed. The same secure approach is used for:
- Admin users created during application seeding
- Reader users who register through the UI
- Any users created through API endpoints

### Managing User Secrets

To update the admin password:

```bash
cd src/Presentation/Blog.WebApi
dotnet user-secrets set "AdminCredentials:Password" "YourNewSecurePassword"
```

For production environments, consider using:
- Azure Key Vault
- Environment variables in Docker
- Other secure secret management solutions

## Authorization System

The application uses a hybrid approach for authorization:
1. ASP.NET Core Identity's role-based authorization
2. Custom endpoint-based permissions

This allows for fine-grained control over API access based on user roles and specific endpoints.

## Setup in Development Environment

To set up the admin user in development:

1. The user secrets are already initialized with the project
2. The admin user is created at application startup
3. You can log in with the admin credentials 