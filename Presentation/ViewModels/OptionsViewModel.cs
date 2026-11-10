using bms_manager.Application.Services;
using bms_manager.Presentation.Commands;
using bms_manager.Presentation.Localization;
using bms_manager.Presentation.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace bms_manager.Presentation.ViewModels;

public sealed class OptionsViewModel : INotifyPropertyChanged
{
    private readonly IAppConfigurationService _configurationService;
    private readonly IFolderPickerService _folderPickerService;
    private string _selectedLanguage;
    private string _decompressDir;

    public OptionsViewModel(
        LocalizationProvider localization,
        IAppConfigurationService configurationService,
        IFolderPickerService folderPickerService)
    {
        Localization = localization;
        _configurationService = configurationService;
        _folderPickerService = folderPickerService;
        _selectedLanguage = localization.ResolveLanguageCode(configurationService.GetSelectedLanguage());
        _decompressDir = configurationService.GetDecompressDir();
        Localization.CurrentLanguage = _selectedLanguage;
        BrowseDecompressDirCommand = new RelayCommand(BrowseDecompressDir);
    }

    public LocalizationProvider Localization { get; }
    public RelayCommand BrowseDecompressDirCommand { get; }

    public string DecompressDir
    {
        get => _decompressDir;
        private set
        {
            if (_decompressDir == value)
            {
                return;
            }

            _decompressDir = value;
            OnPropertyChanged();
        }
    }

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

    private void BrowseDecompressDir()
    {
        string? selectedDirectory = _folderPickerService.SelectFolder(
            Environment.ExpandEnvironmentVariables(DecompressDir),
            Localization.DecompressDirectoryLabel);

        if (selectedDirectory is null)
        {
            return;
        }

        _configurationService.SetDecompressDir(selectedDirectory);
        DecompressDir = _configurationService.GetDecompressDir();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
