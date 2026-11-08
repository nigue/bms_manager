using bms_manager.Application.Services;
using bms_manager.Presentation.Localization;
using System.Windows;
using System.Windows.Controls;

namespace bms_manager.Presentation.Views.Sections.SubSections;

public partial class OptionsWindow : Window
{
    private readonly LocalizationProvider _localization;
    private readonly IAppConfigurationService _configurationService;

    public OptionsWindow(LocalizationProvider localization, IAppConfigurationService configurationService)
    {
        _localization = localization;
        _configurationService = configurationService;
        InitializeComponent();
        DataContext = localization;

        string selectedLanguage = localization.ResolveLanguageCode(_configurationService.GetSelectedLanguage());
        _localization.CurrentLanguage = selectedLanguage;
        LanguageComboBox.SelectedValue = selectedLanguage;
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedValue is string language && language != _localization.CurrentLanguage)
        {
            _configurationService.SetSelectedLanguage(language);
            _localization.CurrentLanguage = language;
        }
    }
}
