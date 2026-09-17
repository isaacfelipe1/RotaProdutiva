using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Domain.Interfaces;
using RotaProdutiva.Infrastructure.Data;
using RotaProdutiva.Infrastructure.Repositories;
using RotaProdutiva.Infrastructure.Security;

namespace RotaProdutiva.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IInscricaoRepository, InscricaoRepository>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
