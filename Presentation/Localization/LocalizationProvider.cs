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
            .Where(language => language is not null)
            .Select(language => language!)
            .OrderBy(language => language)
            .ToArray();

        if (AvailableLanguages.Count == 0)
        {
            throw new InvalidOperationException($"No se encontraron archivos de idioma en '{_localizationDirectory}'.");
        }

        _currentLanguage = AvailableLanguages.Contains("es") ? "es" : AvailableLanguages[0];
        _texts = LoadLanguage(_currentLanguage);
    }

    public IReadOnlyList<string> AvailableLanguages { get; }

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (!AvailableLanguages.Contains(value))
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
            OnPropertyChanged(nameof(LanguageLabel));
            OnPropertyChanged(nameof(NameLabel));
            OnPropertyChanged(nameof(SaveButton));
            OnPropertyChanged(nameof(SaveSuccessFormat));
            OnPropertyChanged(nameof(SaveError));
            OnPropertyChanged(nameof(NameRequired));
        }
    }

    public string LanguageLabel => GetText("LanguageLabel");
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
}