using System.Windows;
using System.Windows.Media.Imaging;
using bms_manager.Presentation.ViewModels;

namespace bms_manager.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        Icon = new BitmapImage(new Uri("pack://application:,,,/Assets/icon.ico"));
        DataContext = viewModel;
    }
}