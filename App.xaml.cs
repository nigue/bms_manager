using System.Windows;
using bms_manager.Application.Services;
using bms_manager.Domain.Repositories;
using bms_manager.Infrastructure.Repositories;
using bms_manager.Presentation.ViewModels;
using bms_manager.Presentation.Views;

namespace bms_manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InMemoryNameRepository repository = new InMemoryNameRepository();
            INameRepository repositoryContract = repository;
            INameService nameService = new NameService(repositoryContract);
            MainViewModel viewModel = new MainViewModel(nameService);
            MainWindow window = new MainWindow(viewModel);
            MainWindow = window;
            window.Show();
        }
    }
}
