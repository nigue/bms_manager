using bms_manager.Presentation.ViewModels;
using bms_manager.Presentation.Views.Sections.SubSections;
using System.Windows;
using System.Windows.Media.Imaging;

namespace bms_manager.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel, OptionsViewModel optionsViewModel)
    {
        InitializeComponent();

        Icon = new BitmapImage(new Uri("pack://application:,,,/Assets/icon.ico"));
        DataContext = viewModel;
        PrincipalMenu.OptionsRequested += (_, _) =>
        {
            var optionsWindow = new OptionsWindow(optionsViewModel)
            {
                Owner = this
            };

            optionsWindow.Show();
        };
    }
}