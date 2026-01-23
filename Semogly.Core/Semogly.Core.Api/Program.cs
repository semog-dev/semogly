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

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),

            ClockSkew = TimeSpan.Zero // importante
        };

        // 🔥 Lê o token do COOKIE, não do header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200" // Angular dev
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // se usar cookie ou Authorization header
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
