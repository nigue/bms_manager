using bms_manager.Presentation.Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace bms_manager.Presentation.Localization;

public sealed class LocalizationProvider : INotifyPropertyChanged
{
    private readonly string _localizationDirectory;
    private Dictionary<string, string> _texts = [];
    private string _currentLanguage;

    public LocalizationProvider()
    {
        _localizationDirectory = Path.Combine(AppContext.BaseDirectory, "Localization");

        AvailableLanguages = Directory
            .GetFiles(_localizationDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(code => code is not null)
            .Select(code => new LanguageOption(code!, GetLanguageDisplayName(code!)))
            .OrderBy(language => language.DisplayName)
            .ToArray();

        if (AvailableLanguages.Count == 0)
        {
            throw new InvalidOperationException($"No se encontraron archivos de idioma en '{_localizationDirectory}'.");
        }

        _currentLanguage = AvailableLanguages.Any(language => language.Code == "es")
            ? "es"
            : AvailableLanguages[0].Code;
        _texts = LoadLanguage(_currentLanguage);
    }

    public IReadOnlyList<LanguageOption> AvailableLanguages { get; }
    public string ConfigurationMenuLabel => GetText("ConfigurationMenuLabel");
    public string OptionsSubMenuLabel => GetText("OptionsSubMenuLabel");
    public string OptionsWindowTitle => GetText("OptionsWindowTitle");
    public string HelpMenuLabel => GetText("HelpMenuLabel");
    public string VersionSubMenuLabel => GetText("VersionSubMenuLabel");

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (AvailableLanguages.All(language => language.Code != value))
            {
                throw new ArgumentException($"El idioma '{value}' no está disponible.", nameof(value));
            }

            if (_currentLanguage == value)
            {
                return;
            }

            Dictionary<string, string> newTexts = LoadLanguage(value);

            _currentLanguage = value;
            _texts = newTexts;

            OnPropertyChanged();
            OnPropertyChanged(nameof(ConfigurationMenuLabel));
            OnPropertyChanged(nameof(OptionsSubMenuLabel));
            OnPropertyChanged(nameof(OptionsWindowTitle));
            OnPropertyChanged(nameof(HelpMenuLabel));
            OnPropertyChanged(nameof(VersionSubMenuLabel));
            OnPropertyChanged(nameof(LanguageLabel));
            OnPropertyChanged(nameof(BmsEventLabel));
            OnPropertyChanged(nameof(NameLabel));
            OnPropertyChanged(nameof(SaveButton));
            OnPropertyChanged(nameof(SaveSuccessFormat));
            OnPropertyChanged(nameof(SaveError));
            OnPropertyChanged(nameof(NameRequired));
        }
    }

    public string LanguageLabel => GetText("LanguageLabel");
    public string BmsEventLabel => GetText("BmsEventLabel");
    public string NameLabel => GetText("NameLabel");
    public string SaveButton => GetText("SaveButton");
    public string SaveSuccessFormat => GetText("SaveSuccessFormat");
    public string SaveError => GetText("SaveError");
    public string NameRequired => GetText("NameRequired");

    public event PropertyChangedEventHandler? PropertyChanged;

    private Dictionary<string, string> LoadLanguage(string language)
    {
        string filePath = Path.Combine(_localizationDirectory, $"{language}.json");
        string json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
            ?? throw new InvalidDataException($"El archivo de idioma '{filePath}' no contiene textos válidos.");
    }

    private string GetText(string key)
    {
        return _texts.TryGetValue(key, out string? text)
            ? text
            : throw new InvalidDataException($"Falta la clave '{key}' en el archivo del idioma '{_currentLanguage}'.");
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private static string GetLanguageDisplayName(string code) => code switch
    {
        "en" => "English",
        "es" => "Spanish",
        "ja" => "Japanese",
        "ko" => "Korean",
        "zh" => "Chinese",
        _ => throw new InvalidDataException($"No hay un nombre visible configurado para el idioma '{code}'.")
    };
}