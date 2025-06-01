

- This Blog Application basically consists of two use cases where only Admin user can login to use Admin Dashboard to create a Post and Reader users where they can use it to read the blogposts. (Commenting functionality will be developed later)
- Main purpose of developing such a basic project on Onion Architecture or some other design choices which might seem to be overengineering at first, is to practice the following development technologies, methods or libraries by implementing the best practices around the idea of 'How can we implement a Medium-Like application' if we were to design it.
   * [Fluent Api](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties) to configure data models between application and database.
   * [Fluent Validation](https://docs.fluentvalidation.net/en/latest/aspnet.html) for request validation.
   * [MediatR](https://github.com/jbogard/MediatR) setup for transferring Request-Response between Application and Presentation layers as well as setting up a common validation pipeline combining the fluent validation.
   * [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs) Pattern for preparing the ground to better scalability for an application where the load on Read operations will be much more compared to write operations.
   * [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio) for role based authentication and combined with [JWT](https://jwt.io/introduction) for token-based authentication.

# Project Structure:

At the Core Layer:
- Application project which defines the Abstractions in terms of Data Access Repositories and Services
- Domain Project that keeps the definitions of the entities in the system

At the Infrastructure Layer:
- Persistance project which implements the repositories defined in application layer to create a gradually-increasing dependencies which leads to loosely-coupled layers, instead of highly-coupled systems where most of the implementations occur on the lower levels.
- Database operations and context configurations.

Client Applications/Consumers
- Three is one Angular consumer application in this layer which will be an entry point for API CRUD operations.

Why CQRS: Seperating Commands and Queries has several benefits such as:
- It gives us the flexibility of optimizing and scaling our the read and write operations. We may consider seperating our write database and read database since read and write operations usually have different expectations when it comes to scaling. Since it's a BlogPost website most of the database accesses will be read-heavy use cases. As the audience grows, so the need for seperating the read and write servers, that's why CQRS might be a good idea.
- Another benefit is the ability to do all the mappings and conversions between actual models and DTOs in the Application layer which Clean Code/Clean Architecture principles advise us to do. CQRS fits Onion Architecture perfectly well in this manner.


## Authentication and Authorization

### Admin User

The application comes with a pre-configured admin user. The admin user is automatically created when the application starts if it doesn't already exist.

- Username: `admin`
- Email: `admin@gmail.com`
- Password: Stored in user secrets

The admin password is stored in user secrets for security reasons and not hardcoded in the source code.

### Password Security

All passwords in the application are securely stored using ASP.NET Core Identity's password hashing. Passwords are:

1. Never stored in plain text and only password hashes are stored
2. Hashed using strong cryptographic algorithms (PBKDF2 with HMAC-SHA256)
3. Protected with a unique salt for each user to prevent rainbow table attacks
4. The plaintext Password field has been removed from the User entity in favor of only storing PasswordHash

Even administrators cannot see users' passwords as they are one-way hashed. The same secure approach is used for:
- Admin users created during application seeding
- Reader users who register through the UI
- Any users created through API endpoints

### Managing User Secrets

To update the admin password: (you should set your own before running the app)

```bash
cd src/Presentation/Blog.WebApi
dotnet user-secrets set "AdminCredentials:Password" "YourNewSecurePassword"
```

## Authorization System

The application uses a hybrid approach for authorization:
1. ASP.NET Core Identity's role-based authorization
2. Custom endpoint-based permissions

## Setup in Development Environment

To set up the admin user in development:

1. The user secrets are already initialized with the project
2. The admin user is created at application startup
3. You can log in with the admin credentials 
