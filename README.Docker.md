# Docker Setup for Blog Application

This document explains how to set up and run the Blog application using Docker.

## Prerequisites

- Docker and Docker Compose installed on your system
- Git to clone the repository

## Getting Started

Follow these steps to set up the application:

1. Clone the repository:
   ```
   git clone <repository-url>
   cd MyOwnBlog
   ```

2. Set up the database password:
   
   The application uses a secure way to store the SQL Server password using Docker secrets. By default, it looks for a password file in `./secrets/db_password.txt`.

   ```
   mkdir -p secrets
   echo "YourSecurePassword" > secrets/db_password.txt
   chmod 600 secrets/db_password.txt  # Restrict access to the file
   ```

3. Start the application:
   ```
   docker compose up -d
   ```

   This command will:
   - Build the application image
   - Start the SQL Server container
   - Wait for the database to be ready
   - Apply migrations automatically
   - Start the application

4. Access the application:
   - API (Backend UI): [Swagger](https://localhost:7165/swagger/index.html)
   - Front-End UI: [Angular UI](http://localhost:4200/blog)

## Environment Configuration

The Docker Compose setup uses environment variables and Docker secrets to configure the application:

- Database connection uses Docker secrets for secure password management
- The connection string is set as an environment variable
- The application automatically runs migrations on startup

## Troubleshooting

If you encounter issues:

Manually try connecting to the database:
   ```
   docker exec -it blog_db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $(cat secrets/db_password.txt)
   ```

## Development Workflow

For local development:

1. Use Docker for SQL Server only:
   ```
   docker compose up db -d
   ```

2. Update your local connection string to point to the Docker database:
   - Server: localhost,1433
   - User: sa
   - Password: (contents of secrets/db_password.txt)

3. Run the application locally from your IDE or command line.

## Security Considerations

- The database password is stored securely using Docker secrets
- The application container runs as a non-root user
- All sensitive information is passed via environment variables or secrets

### Building and running your application

When you're ready, start your application by running:
`docker compose up --build`.

Then, push it to your registry, e.g. `docker push myregistry.com/myapp`.

Consult Docker's [getting started](https://docs.docker.com/go/get-started-sharing/)
docs for more detail on building and pushing.
