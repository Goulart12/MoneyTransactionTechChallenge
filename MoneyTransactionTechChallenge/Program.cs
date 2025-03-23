using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyTransactionTechChallenge.Helpers.AuthHelpers;
using MoneyTransactionTechChallenge.Helpers.CacheHelpers;
using MoneyTransactionTechChallenge.Helpers.DataHelpers;
using MoneyTransactionTechChallenge.Repositories;
using MoneyTransactionTechChallenge.Repositories.FinancialTransactions;
using MoneyTransactionTechChallenge.Services;
using MoneyTransactionTechChallenge.Services.FinancialTransactions;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
        
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

var configuration = builder.Configuration;

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
                .GetBytes(configuration["ApplicationSettings:JWT_Secret"])
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "MoneyTransactionTechDemo_"; // Optional prefix for cache keys
});

var services = builder.Services;

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

services.AddSingleton<DataContext>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IAuthHelper, AuthHelper>();
services.AddScoped<IRedisCacheService, RedisCacheService>();
services.AddScoped<IWalletRepository, WalletRepository>();
services.AddScoped<IFinancialTransactionService, FinancialTransactionService>();

var app = builder.Build();

{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();