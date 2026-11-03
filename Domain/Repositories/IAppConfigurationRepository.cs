using bms_manager.Domain.Model;

namespace bms_manager.Domain.Repositories;

public interface IAppConfigurationRepository
{
    AppConfiguration GetConfiguration();
    void SaveConfiguration(AppConfiguration appConfiguration);
}