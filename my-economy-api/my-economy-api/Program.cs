using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using my_economy_api.Data;
using my_economy_api.Services;
using my_economy_api.Services.Interfaces;
using RepositoryPatern;
using RepositoryPatern.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext with the dependency injection container, configuring it to use SQL Server with the connection string specified in the application's configuration settings.
// This allows the application to interact with the database using Entity Framework Core for data access operations.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the DbContext as a scoped service in the dependency injection container, allowing it to be injected into other services and controllers that require database access.
builder.Services.AddScoped<DbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

// Retrieve the Supabase URL and key from the application's configuration settings and register a new instance of the Supabase client in the dependency injection container,
// allowing it to be injected into other services and controllers that require access to Supabase for authentication and data operations.
var supabaseUrl = builder.Configuration["Supabase:Url"]
    ?? throw new InvalidOperationException("Supabase URL is missing.");
var supabaseKey = builder.Configuration["Supabase:Key"]
    ?? throw new InvalidOperationException("Supabase Key is missing.");

builder.Services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey));
builder.Services.AddScoped<IAuthService, SupabaseAuthService>();

// Register the generic repository interface and its implementation in the dependency injection container, allowing for the use of the repository pattern throughout the application for data access operations.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Configure JWT authentication, specifying the token validation parameters to ensure that incoming JWT tokens are properly validated against the expected issuer, audience, signing key, and expiration time.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSecret = builder.Configuration["Supabase:JwtSecret"]
            ?? throw new InvalidOperationException("JWT Secret is missing.");
        var supabaseUrl = builder.Configuration["Supabase:Url"]
            ?? throw new InvalidOperationException("Supabase URL is missing.");

        var key = Encoding.ASCII.GetBytes(jwtSecret);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateIssuer = true,
            // Importante: Supabase espera este formato exacto para el Issuer
            ValidIssuer = $"{supabaseUrl.TrimEnd('/')}/auth/v1",

            ValidateAudience = true,
            ValidAudience = "authenticated",

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Opcional: elimina el margen de 5 min por defecto
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment()) //To avoid the warnings in tests
{
    app.UseHttpsRedirection();
}

app.UseRouting();  

app.UseCors("AllowLocalhost"); 

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();      

app.Run();

public partial class Program { }
