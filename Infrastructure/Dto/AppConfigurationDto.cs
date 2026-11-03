using bms_manager.Domain.Model;

namespace bms_manager.Infrastructure.Dto;

public record AppConfigurationDto(
    string DownloadDir,
    string SelectedLanguage,
    string DecompressDir
)
{
    public AppConfiguration ToModel() => new (
        DownloadDir,
        SelectedLanguage,
        DecompressDir);
}