using bms_manager.Presentation.Localization;
using bms_manager.Presentation.Services;
using bms_manager.Presentation.Services.Implementations;
using bms_manager.Presentation.ViewModels;
using bms_manager.Presentation.Views;
using Microsoft.Extensions.DependencyInjection;

namespace bms_manager.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSingleton<LocalizationProvider>();
        services.AddTransient<IFolderPickerService, FolderPickerService>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<OptionsViewModel>();
        services.AddTransient<MainWindow>();

        return services;
    }
}
