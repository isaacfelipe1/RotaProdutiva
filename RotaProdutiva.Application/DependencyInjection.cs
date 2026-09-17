using Microsoft.Extensions.DependencyInjection;
using RotaProdutiva.Application.Interfaces;
using RotaProdutiva.Application.Services;

namespace RotaProdutiva.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICursoService, CursoService>();
            services.AddScoped<IInscricaoService, InscricaoService>();
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }
    }
}
