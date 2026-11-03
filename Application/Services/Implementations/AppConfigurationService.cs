using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;

namespace bms_manager.Application.Services.Implementations;

public sealed class AppConfigurationService : IAppConfigurationService
{
    private readonly IAppConfigurationRepository _repository;

    public AppConfigurationService(IAppConfigurationRepository repository)
    {
        _repository = repository;
    }

    public string GetDownloadDir()
    {
        return _repository.GetConfiguration().DownloadDir;
    }

    public string GetSelectedLanguage()
    {
        return _repository.GetConfiguration().SelectedLanguage;
    }

    public string GetDecompressDir()
    {
        return _repository.GetConfiguration().DecompressDir;
    }

    public void SetDownloadDir(string downloadDir)
    {
        AppConfiguration configuration = _repository.GetConfiguration();
        _repository.SaveConfiguration(configuration with { DownloadDir = downloadDir });
    }

    public void SetSelectedLanguage(string selectedLanguage)
    {
        AppConfiguration configuration = _repository.GetConfiguration();
        _repository.SaveConfiguration(configuration with { SelectedLanguage = selectedLanguage });
    }

    public void SetDecompressDir(string decompressDir)
    {
        AppConfiguration configuration = _repository.GetConfiguration();
        _repository.SaveConfiguration(configuration with { DecompressDir = decompressDir });
    }
}
