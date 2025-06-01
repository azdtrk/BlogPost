using Blog.Application;
using Blog.Application.Middleware;
using Blog.Infrastructure;
using Blog.Persistance;
using Blog.Persistance.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add support for Docker environment variables and user secrets
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add user secrets explicitly for development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configure Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(logger);

// Add services to the container.
// Configure JSON serialization and controllers first
builder.Services
    .AddControllers(options => 
    {
        options.Filters.Add<Blog.WebApi.Filters.RolePermissionFilter>();
    })
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .AddFluentValidation();

// Then add API explorer and Swagger
builder.Services.AddEndpointsApiExplorer();

// Simplified Swagger configuration
builder.Services.AddSwaggerGen(c => 
{
    c.CustomSchemaIds(type => type.FullName);
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1"
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

// Register application services (includes MediatR, AutoMapper, and Validators)
builder.Services.AddApplicationServices();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

// Configure CORS - allow requests from the Angular app and Swagger
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",     // Angular app
                "https://localhost:4200",
                "http://localhost:5000",
                "https://localhost:5000",
                "http://localhost:5001",
                "https://localhost:5001",
                "http://127.0.0.1:4200",
                "https://127.0.0.1:4200",
                "https://localhost:7165"     // Add the Swagger UI origin
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("Content-Disposition")
              .SetIsOriginAllowed(_ => true); // For development only - remove in production
    });
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name
        };
    })
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

// Apply migrations automatically in Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    Log.Information("Running in Docker, applying migrations automatically");
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        Log.Information("Migrations applied successfully");
    }
}

// Seed authorization endpoints
try
{
    Log.Information("Seeding endpoint authorization data");
    using (var scope = app.Services.CreateScope())
    {
        await SeedData.SeedEndpointsAsync(app.Services);
    }
    Log.Information("Endpoint authorization data seeded successfully");
    
    // Seed admin user
    Log.Information("Seeding admin user");
    using (var scope = app.Services.CreateScope())
    {
        await SeedData.SeedAdminUserAsync(app.Services);
    }
    Log.Information("Admin user seeded successfully");
}
catch (Exception ex)
{
    Log.Error(ex, "An error occurred while seeding data");
}

// Ordering is important for middleware!
app.UseHttpsRedirection();

// Apply CORS before routing and other middleware
app.UseCors("AllowAngularApp");

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
