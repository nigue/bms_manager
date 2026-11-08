using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace bms_manager.Presentation.Views.Sections;

public partial class PrincipalMenuView : UserControl
{
    public PrincipalMenuView()
    {
        InitializeComponent();
    }

    public event EventHandler? OptionsRequested;

    private void ShowOptions_Click(object sender, RoutedEventArgs e)
    {
        OptionsRequested?.Invoke(this, EventArgs.Empty);
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
