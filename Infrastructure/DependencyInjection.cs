using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace bms_manager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<INameRepository, InMemoryNameRepository>();
        services.AddSingleton<IBmsEventRepository, InMemoryBmsEventRepository>();
        services.AddSingleton<IAppConfigurationRepository, AppConfigurationRepository>();

        return services;
    }
}
