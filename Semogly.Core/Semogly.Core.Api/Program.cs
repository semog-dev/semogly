using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Semogly.Core.Api.SharedContext.Common;
using Semogly.Core.Application;
using Semogly.Core.Data;
using Semogly.Core.Domain;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

// Add services to the container.

builder.Services.AddDbContexts();
builder.Services.AddUnitOfWorks();
builder.Services.AddRepositories();

builder.Services.AddMediator();
builder.Services.AddValidations();

builder.Services.AddServices();

builder.Services.AddProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // em produção use true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = Configuration.Jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = Configuration.Jwt.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.Jwt.Key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
