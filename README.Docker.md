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

   > ⚠️ **Security Note**: For production environments, never commit the password file to version control. Add `secrets/` to your `.gitignore` file.

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
   - API: http://localhost:8080
   - Swagger UI: http://localhost:8080/swagger

## Environment Configuration

The Docker Compose setup uses environment variables and Docker secrets to configure the application:

- Database connection uses Docker secrets for secure password management
- The connection string is set as an environment variable
- The application automatically runs migrations on startup

## Troubleshooting

If you encounter issues:

1. Check container logs:
   ```
   docker compose logs api
   docker compose logs db
   ```

2. Ensure the SQL Server container is healthy:
   ```
   docker compose ps
   ```

3. Manually connect to the database:
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

Your application will be available at http://localhost:8080.

### Deploying your application to the cloud

First, build your image, e.g.: `docker build -t myapp .`.
If your cloud uses a different CPU architecture than your development
machine (e.g., you are on a Mac M1 and your cloud provider is amd64),
you'll want to build the image for that platform, e.g.:
`docker build --platform=linux/amd64 -t myapp .`.

Then, push it to your registry, e.g. `docker push myregistry.com/myapp`.

Consult Docker's [getting started](https://docs.docker.com/go/get-started-sharing/)
docs for more detail on building and pushing.

### References
* [Docker's .NET guide](https://docs.docker.com/language/dotnet/)
* The [dotnet-docker](https://github.com/dotnet/dotnet-docker/tree/main/samples)
  repository has many relevant samples and docs.