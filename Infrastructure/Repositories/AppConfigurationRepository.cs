using System.IO;
using System.Text.Json;
using bms_manager.Domain.Model;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Dto;

namespace bms_manager.Infrastructure.Repositories;

public sealed class AppConfigurationRepository : IAppConfigurationRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };

    private readonly string _settingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "BmsManager",
        "settings.json");

    public AppConfiguration GetConfiguration()
    {
        string json = File.ReadAllText(_settingsFilePath);
        AppConfigurationDto dto = JsonSerializer.Deserialize<AppConfigurationDto>(json, JsonOptions)
            ?? throw new InvalidDataException("The settings file does not contain a configuration.");

        return dto.ToModel();
    }

    public void SaveConfiguration(AppConfiguration appConfiguration)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath)!);

        string json = JsonSerializer.Serialize(appConfiguration.ToDto(), JsonOptions);
        File.WriteAllText(_settingsFilePath, json);
    }
}
