using System.Text;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RaoCattles.Api.Middleware;
using RaoCattles.BuildingBlocks.Authentication;
using RaoCattles.BuildingBlocks.Cloudinary;
using RaoCattles.BuildingBlocks.Mongo;
using RaoCattles.Modules.Products.Infrastructure;
using RaoCattles.Modules.Users.Infrastructure;
using RaoCattles.Modules.Users.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// -- Railway port binding ---
var port = Environment.GetEnvironmentVariable("PORT");
if (port is not null)
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// -- MongoDB -- ──────────────────────────────────────────────────────────────
var mongoSettings = builder.Configuration.GetSection("MongoDB").Get<MongoSettings>()!;

if (string.IsNullOrWhiteSpace(mongoSettings.ConnectionString))
    throw new InvalidOperationException(
        "MongoDB:ConnectionString is not configured. " +
        "Set the MongoDB__ConnectionString environment variable.");

var mongoClient = new MongoClient(mongoSettings.ConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// ── Cloudinary ───────────────────────────────────────────────────────────
var cloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>()!;
var cloudinaryAccount = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
builder.Services.AddSingleton(new Cloudinary(cloudinaryAccount));

// ── JWT Authentication ───────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ── CORS ─────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ── Module registrations ─────────────────────────────────────────────────
builder.Services.AddProductsModule();
builder.Services.AddUsersModule(jwtSettings);

// ── Controllers ──────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddApplicationPart(typeof(RaoCattles.Modules.Products.Presentation.Controllers.PublicProductsController).Assembly)
    .AddApplicationPart(typeof(RaoCattles.Modules.Users.Presentation.Controllers.AuthController).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

// ── Seed admin user ──────────────────────────────────────────────────────
var adminUsername = builder.Configuration["AdminSeed:Username"] ?? "Admin";
var adminPassword = builder.Configuration["AdminSeed:Password"] ?? "raocattle0331";
await AdminSeeder.SeedAsync(mongoDatabase, adminUsername, adminPassword);

// -- Middleware pipeline --------------------------------------------------
app.UseExceptionHandler();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
