using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Semogly.Core.Api.SharedContext.Common;
using Semogly.Core.Api.SharedContext.Services;
using Semogly.Core.Application;
using Semogly.Core.Application.SharedContext.Services;
using Semogly.Core.Domain;
using Semogly.Core.Domain.SharedContext;
using Semogly.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

builder.Services.AddData();
builder.Services.AddUnitOfWorks();
builder.Services.AddRepositories();

builder.Services.AddMediator();
builder.Services.AddValidations();

builder.Services.AddServices();

builder.Services.AddProviders();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentSessionService, CurrentSessionService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
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

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
