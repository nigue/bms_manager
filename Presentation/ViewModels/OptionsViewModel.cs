using bms_manager.Application.Services;
using bms_manager.Presentation.Localization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace bms_manager.Presentation.ViewModels;

public sealed class OptionsViewModel : INotifyPropertyChanged
{
    private readonly IAppConfigurationService _configurationService;
    private string _selectedLanguage;

    public OptionsViewModel(LocalizationProvider localization, IAppConfigurationService configurationService)
    {
        Localization = localization;
        _configurationService = configurationService;
        _selectedLanguage = localization.ResolveLanguageCode(configurationService.GetSelectedLanguage());
        Localization.CurrentLanguage = _selectedLanguage;
    }

    public LocalizationProvider Localization { get; }

    public string SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            string language = Localization.ResolveLanguageCode(value);
            if (_selectedLanguage == language)
            {
                return;
            }

            _configurationService.SetSelectedLanguage(language);
            Localization.CurrentLanguage = language;
            _selectedLanguage = language;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
