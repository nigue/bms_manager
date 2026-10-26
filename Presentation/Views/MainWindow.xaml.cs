using System.Windows;
using bms_manager.Presentation.ViewModels;

namespace bms_manager.Presentation.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}