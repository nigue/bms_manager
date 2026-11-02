using bms_manager.Application.Services;
using bms_manager.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace bms_manager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<INameService, NameService>();

        return services;
    }
}
