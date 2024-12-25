using Api.Application.Config;
using Api.Application.Interfaces;
using Api.Application.Services;
using Api.Domain.Interfaces;
using Api.Infrastructure.Data;
using Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite requisições de qualquer origem
              .AllowAnyMethod()  // Permite qualquer método HTTP
              .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

// Configuração do JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

// Configuração de autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var message = "Token inválido ou expirado. Por favor, faça login novamente.";
            var result = JsonSerializer.Serialize(new { message });
            return context.Response.WriteAsync(result);
        },
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var message = "Falha na autenticação. Verifique o token ou suas credenciais.";
            var result = JsonSerializer.Serialize(new { message });
            return context.Response.WriteAsync(result);
        }
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;

            if (token != null)
            {
                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                var isRevoked = await userService.ValidateTokenAsync(token.RawData);

                if (!isRevoked)
                {
                    context.Fail("Token revogado.");
                }
            }
        }
    };
});

// Adicionar serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Definir a segurança para o Swagger UI (Botão de Authorize)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Informe o token JWT no formato: Bearer {seu_token}"
    });

    // Adicionar a exigência de segurança global para todas as requisições
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Configurar o DbContext para o SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios e serviços
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Adicionar o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar o middleware CORS
app.UseCors("AllowAll");

// Configurando o middleware de autenticação
app.UseAuthentication();
app.UseAuthorization();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
