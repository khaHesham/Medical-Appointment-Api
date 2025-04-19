using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MedicalAppointmentApi.Models.Entities;
using MedicalAppointmentApi.Models.Data;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MedicalAppointmentApi.Middleware;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load(".env");

// Add authentication by default to all controllers
builder.Services.AddControllers(options =>
{
    var globalPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(globalPolicy));
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with connection string from env
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")));

Console.WriteLine(Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING"));


// Add repository and unit of work services
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Add services for authentication and authorization
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddHostedService<AppointmentReminderService>();



// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// JWT Authentication
var jwtSecret = builder.Configuration["JWT_SECRET"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!))
        };

        // Optional: Custom error responses
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Authentication failed.");
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Unauthorized: Token is missing or invalid.");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                return context.Response.WriteAsync("Forbidden: You don’t have access.");
            }
        };
    });

builder.Services.AddAuthorization();


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

// Build the app 
var app = builder.Build();

// apply migrations if needed
// Automatically apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // if the database does not exist, it will be created
    db.Database.Migrate();
}


// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
