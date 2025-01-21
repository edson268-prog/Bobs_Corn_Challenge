using Bobs_Corn_Challenge.DataAccess.Repositories;
using Bobs_Corn_Challenge.Services.Implementations;
using Bobs_Corn_Challenge.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Bobs_Corn_Challenge.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICornRepository, CornRepository>();
            services.AddScoped<ICornService, CornService>();
            return services;
        }
    }
}
