using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ProjectKy3.Data;
using System.IO;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Connect to the database
builder.Services.AddDbContext<ProjectKy3DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectKy3")));

// Set up Data Protection for encrypting sensitive data
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"./keys")) // Securely store encryption keys
    .SetApplicationName("ProjectKy3")                      // Set unique app name for key management
    .SetDefaultKeyLifetime(TimeSpan.FromDays(14));          // Rotate keys every 14 days

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure Cookie Authentication with enhanced security
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/Auth/login";   // Redirect if user is not authenticated
        options.AccessDeniedPath = "/api/Auth/accessdenied"; // Redirect if access is denied
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized; // API should not redirect, return 401
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
            return Task.CompletedTask;
        };
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Set cookie expiration to 7 days
        options.SlidingExpiration = true;  // Extend expiration time with user activity
        options.Cookie.HttpOnly = true;    // Prevent client-side access to cookies via JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Send cookies only over HTTPS
        options.Cookie.SameSite = SameSiteMode.None;  // Cross-site cookies needed for localhost:3000 <-> API
    });

builder.Services.AddAuthorization();

// Add CORS policy to allow requests from http://localhost:3000 with support for cookies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();  // Allow cookies to be sent with cross-origin requests
        });
});

// Add support for NewtonsoftJson (optional)
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS globally with the policy defined above
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
