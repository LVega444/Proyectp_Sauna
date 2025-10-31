using Microsoft.Extensions.DependencyInjection;
using ProyectoSauna.Services.Interfaces;
using ProyectoSauna.Services;

namespace ProyectoSauna.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registrar servicios de aplicación
            services.AddScoped<IAuthService, AuthService>();
            
            return services;
        }
    }
}