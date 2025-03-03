# syntax=docker/dockerfile:1

# Comments are provided throughout this file to help you get started.
# If you need more help, visit the Dockerfile reference guide at
# https://docs.docker.com/go/dockerfile-reference/

# Want to help us make this template better? Share your feedback here: https://forms.gle/ybq9Krt8jtBL3iCk7

################################################################################

# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md

# Create a stage for building the application.
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

WORKDIR /source

# Copy the solution and project files first to optimize Docker cache
COPY *.sln ./
COPY src/Core/Blog.Application/*.csproj ./src/Core/Blog.Application/
COPY src/Core/Blog.Domain/*.csproj ./src/Core/Blog.Domain/
COPY src/Infrastructure/Blog.Infrastructure/*.csproj ./src/Infrastructure/Blog.Infrastructure/
COPY src/Infrastructure/Blog.Persistance/*.csproj ./src/Infrastructure/Blog.Persistance/
COPY src/Presentation/Blog.WebApi/*.csproj ./src/Presentation/Blog.WebApi/

# Restore packages
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Build the application
WORKDIR /source/src/Presentation/Blog.WebApi
RUN dotnet publish -c Release -o /app --no-restore

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Install dependencies for sed command and SQL tools
RUN apk add --no-cache sed bash

# Set environment variable to indicate we're in Docker
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy published files
COPY --from=build /app ./

# Set proper ownership
RUN chmod +x /app/Blog.WebApi.dll

# Switch to non-root user for security
USER $APP_UID

# Expose port
EXPOSE 80

ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]
