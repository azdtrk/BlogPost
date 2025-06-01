# syntax=docker/dockerfile:1

# Comments are provided throughout this file to help you get started.
# If you need more help, visit the Dockerfile reference guide at
# https://docs.docker.com/go/dockerfile-reference/

# Want to help us make this template better? Share your feedback here: https://forms.gle/ybq9Krt8jtBL3iCk7

################################################################################

# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# Create a stage for building the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS dotnet-build

WORKDIR /source

# Copy only the .NET project files first to optimize Docker cache
COPY src/Core/Blog.Application/*.csproj ./src/Core/Blog.Application/
COPY src/Core/Blog.Domain/*.csproj ./src/Core/Blog.Domain/
COPY src/Infrastructure/Blog.Infrastructure/*.csproj ./src/Infrastructure/Blog.Infrastructure/
COPY src/Infrastructure/Blog.Persistance/*.csproj ./src/Infrastructure/Blog.Persistance/
COPY src/Presentation/Blog.WebApi/*.csproj ./src/Presentation/Blog.WebApi/

# Create a temporary solution file with only .NET projects
RUN dotnet new sln --name Blog && \
    dotnet sln add src/Core/Blog.Application/Blog.Application.csproj && \
    dotnet sln add src/Core/Blog.Domain/Blog.Domain.csproj && \
    dotnet sln add src/Infrastructure/Blog.Infrastructure/Blog.Infrastructure.csproj && \
    dotnet sln add src/Infrastructure/Blog.Persistance/Blog.Persistance.csproj && \
    dotnet sln add src/Presentation/Blog.WebApi/Blog.WebApi.csproj

# Restore packages
RUN dotnet restore

# Copy the rest of the .NET code
COPY src/Core/ ./src/Core/
COPY src/Infrastructure/ ./src/Infrastructure/
COPY src/Presentation/ ./src/Presentation/

# Build the application
WORKDIR /source/src/Presentation/Blog.WebApi
RUN dotnet publish -c Release -o /app --no-restore

# Create a stage for building the Angular application
FROM node:18-alpine AS angular-build

WORKDIR /angular

# Copy package.json and package-lock.json for better Docker cache
COPY src/Clients/Blog.UI/package*.json ./

# Install dependencies (including devDependencies for build)
RUN npm ci

# Copy Angular source code
COPY src/Clients/Blog.UI/ ./

# Build Angular application for production
RUN npm run build

# Final stage - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Install dependencies for sed command, bash, and ICU libraries for globalization
RUN apk add --no-cache sed bash icu-libs

# Set environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Copy .NET published files
COPY --from=dotnet-build /app ./

# Copy Angular built files to wwwroot
COPY --from=angular-build /angular/dist/blog.ui ./wwwroot

# Set proper ownership and permissions
RUN chmod +x /app/Blog.WebApi.dll

# Switch to non-root user for security
USER $APP_UID

# Expose port
EXPOSE 80

ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]
