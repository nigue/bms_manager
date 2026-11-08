using bms_manager.Presentation.Localization;
using System.Windows;
using System.Windows.Controls;

namespace bms_manager.Presentation.Views.Sections.SubSections;

public partial class OptionsWindow : Window
{
    private readonly LocalizationProvider _localization;

    public OptionsWindow(LocalizationProvider localization)
    {
        _localization = localization;
        InitializeComponent();
        DataContext = localization;
        LanguageComboBox.SelectedValue = localization.CurrentLanguage;
    }

    private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LanguageComboBox.SelectedValue is string language
            && language != _localization.CurrentLanguage)
        {
            _localization.CurrentLanguage = language;
        }
    }
}
