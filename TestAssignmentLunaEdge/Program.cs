using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using TestAssignmentLunaEdge.Authentication;
using TestAssignmentLunaEdge.Core.Interfaces;
using TestAssignmentLunaEdge.Core.Services;
using TestAssignmentLunaEdge.DataAccess;
using TestAssignmentLunaEdge.DataAccess.Interfaces;
using TestAssignmentLunaEdge.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adding DbContext to the DI container for database interaction using MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),  // Get the connection string from the configuration
        new MySqlServerVersion(new Version(8, 0, 40)) // Specify the MySQL version for compatibility
    ));

// Setting up Swagger for API documentation and adding JWT authorization support
builder.Services.AddSwaggerGen(c =>
{
    // Define the API documentation version and title
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add a security definition for Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",  // Description for how the Bearer token should be used
        Type = SecuritySchemeType.Http,  // Define it as an HTTP authentication scheme
        Scheme = "bearer"  // Specify the bearer scheme for authentication
    });

    // Add a security requirement to enforce Bearer token for the API endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"  // Reference the "Bearer" security definition defined above
                }
            },
            new string[] {} // No additional scopes required for this security scheme
        }
    });
});

// Adding JWT authentication to the application
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // Validate the issuer of the token
            ValidateAudience = true,  // Validate the audience of the token
            ValidateLifetime = true,  // Ensure the token is not expired
            ValidateIssuerSigningKey = true,  // Ensure the token is signed with the correct key
            ValidIssuer = builder.Configuration["Jwt:Issuer"],  // Set the valid issuer from configuration
            ValidAudience = builder.Configuration["Jwt:Audience"],  // Set the valid audience from configuration
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))  // Set the secret key for signing the token
        };
    });

// Registering application services and repositories for dependency injection
builder.Services.AddScoped<IUserService, UserService>();  // Add user service for user-related operations
builder.Services.AddScoped<ITaskService, TaskService>();  // Add task service for task-related operations
builder.Services.AddScoped<IJWTService, JWTService>();  // Add JWT service for handling token generation and validation
builder.Services.AddScoped<IUserRepository, UserRepository>();  // Add user repository for data access
builder.Services.AddScoped<ITaskRepository, TaskRepository>();  // Add task repository for data access


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();  
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
