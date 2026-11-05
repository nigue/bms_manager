using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using bms_manager.Presentation.Views.Sections.SubSections;

namespace bms_manager.Presentation.Views.Sections;

public partial class PrincipalMenuView : UserControl
{
    public PrincipalMenuView()
    {
        InitializeComponent();
    }

    private void ShowOptions_Click(object sender, RoutedEventArgs e)
    {
        var optionsWindow = new OptionsWindow
        {
            DataContext = DataContext,
            Owner = Window.GetWindow(this)
        };

        optionsWindow.Show();
    }

    private void ShowVersion_Click(object sender, RoutedEventArgs e)
    {
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                      ?? "Desconocida";

        MessageBox.Show(
            Window.GetWindow(this),
            $"Versión: {version}",
            "Acerca de",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
