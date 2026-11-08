using System.Windows;
using bms_manager.Presentation.ViewModels;

namespace bms_manager.Presentation.Views.Sections.SubSections;

public partial class OptionsWindow : Window
{
    public OptionsWindow(OptionsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
