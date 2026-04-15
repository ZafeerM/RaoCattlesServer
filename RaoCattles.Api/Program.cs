using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using RaoCattles.BuildingBlocks.Authentication;
using RaoCattles.BuildingBlocks.Mongo;
using RaoCattles.Modules.Products.Infrastructure;
using RaoCattles.Modules.Users.Infrastructure;
using RaoCattles.Modules.Users.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// ── MongoDB ──────────────────────────────────────────────────────────────
var mongoSettings = builder.Configuration.GetSection("MongoDB").Get<MongoSettings>()!;
var mongoClient = new MongoClient(mongoSettings.ConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoSettings.DatabaseName);
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

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
var adminUsername = builder.Configuration["AdminSeed:Username"] ?? "admin";
var adminPassword = builder.Configuration["AdminSeed:Password"] ?? "admin";
await AdminSeeder.SeedAsync(mongoDatabase, adminUsername, adminPassword);

// ── Middleware pipeline ──────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
