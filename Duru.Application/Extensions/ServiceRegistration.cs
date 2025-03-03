using Duru.Application.Interfaces;
using Duru.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Duru.Application.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRoomService, RoomService>();
            // vs...

            return services;
        }
    }
} 