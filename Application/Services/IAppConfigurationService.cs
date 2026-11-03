using bms_manager.Domain.Model;

namespace bms_manager.Application.Services;

public interface IAppConfigurationService
{
    string GetDownloadDir();
    string GetSelectedLanguage();
    string GetDecompressDir();
    void SetDownloadDir(string downloadDir);
    void SetSelectedLanguage(string selectedLanguage);
    void SetDecompressDir(string decompressDir);
}