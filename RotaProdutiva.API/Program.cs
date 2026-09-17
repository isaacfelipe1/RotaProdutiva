using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RotaProdutiva.Application;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Entities;
using RotaProdutiva.Domain.Enums;
using RotaProdutiva.Infrastructure;
using RotaProdutiva.Infrastructure.Data;
using RotaProdutiva.Infrastructure.Security;

namespace RotaProdutiva.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RotaProdutiva API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Informe o token JWT no formato: Bearer {seu token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings não configurado.");

            builder.Services
                .AddAuthentication(options =>
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
                        ValidIssuer = jwtSettings.Emissor,
                        ValidAudience = jwtSettings.Audiencia,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Chave))
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

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();

                SeedAdmin(scope.ServiceProvider, app.Configuration);
            }

            app.Run();
        }

        private static void SeedAdmin(IServiceProvider services, IConfiguration configuration)
        {
            var db = services.GetRequiredService<AppDbContext>();
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();

            var adminEmail = configuration["AdminSeed:Email"];

            if (string.IsNullOrWhiteSpace(adminEmail))
                return;

            if (db.Usuarios.Any(u => u.Email == adminEmail.Trim().ToLower()))
                return;

            var nome = configuration["AdminSeed:Nome"] ?? "Administrador";
            var senha = configuration["AdminSeed:Senha"] ?? "Admin@123";
            var whatsApp = configuration["AdminSeed:WhatsApp"] ?? "";

            var senhaHash = passwordHasher.HashSenha(senha);
            var admin = new Usuario(nome, adminEmail, senhaHash, whatsApp, TipoUsuario.Admin);

            db.Usuarios.Add(admin);
            db.SaveChanges();
        }
    }
}
