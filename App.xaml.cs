using System.Windows;
using bms_manager.Application;
using bms_manager.Infrastructure;
using bms_manager.Presentation;
using bms_manager.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace bms_manager
{
    public partial class App : System.Windows.Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services
                        .AddApplication()
                        .AddInfrastructure()
                        .AddPresentation();
                })
                .Build();

            _host.Start();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_host is not null)
            {
                try
                {
                    _host.StopAsync().GetAwaiter().GetResult();
                }
                finally
                {
                    _host.Dispose();
                }
            }

            base.OnExit(e);
        }
    }
}
