using bms_manager.Infrastructure.Dto;

namespace bms_manager.Domain.Model;

public record AppConfiguration(
    string DownloadDir,
    string SelectedLanguage,
    string DecompressDir
)
{
    public AppConfigurationDto ToDto() => new (
        DownloadDir, 
        SelectedLanguage, 
        DecompressDir);
}