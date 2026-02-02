using BaseCRM.Configurations;
using BaseCRM.DbContexts;
using BaseCRM.Entities;
using BaseCRM.Extensions;
using BaseCRM.Repositories;
using BaseCRM.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(Environment.GetEnvironmentVariable(Constants.DatabaseConnectionString))
    .UseSnakeCaseNamingConvention());

builder.Services.AddDataProtection();



builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
    }
    )
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var randomKey = Guid.NewGuid().ToString();
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable(Constants.JwtIssuer),
        ValidAudience = Environment.GetEnvironmentVariable(Constants.JwtAudience),
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(Constants.JwtKey) ?? randomKey))
    };
});

builder.Services.AddSingleton<IEmailSender, EmailService>();
builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<JWTTokenService>();
builder.Services.AddScoped<AccountService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(Environment.GetEnvironmentVariable(Constants.ClientUrl) ?? "", Environment.GetEnvironmentVariable(Constants.ApplicationUrl) ?? "")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MigrateDatabase<ApplicationDbContext>();

await IdentitySeeder.SeedRolesAndAdminAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

}

app.UseRouting();
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();
