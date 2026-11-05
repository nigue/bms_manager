using bms_manager.Application;
using bms_manager.Infrastructure;
using bms_manager.Presentation;
using bms_manager.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace bms_manager
{
    public partial class App : System.Windows.Application
    {
        private IHost? _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            EnsureSettingsFileExists();

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

        private static void EnsureSettingsFileExists()
        {
            string settingsDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BmsManager");
            Directory.CreateDirectory(settingsDirectory);

            string settingsFilePath = Path.Combine(settingsDirectory, "settings.json");
            if (File.Exists(settingsFilePath))
                return;

            var defaultSettings = new
            {
                download_dir = @"%LocalAppData%\BmsManager\Downloads",
                selected_language = "English",
                decompress_dir = @"%LocalAppData%\BmsManager\Decompress"
            };

            string json = JsonSerializer.Serialize(
                defaultSettings,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsFilePath, json);
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
