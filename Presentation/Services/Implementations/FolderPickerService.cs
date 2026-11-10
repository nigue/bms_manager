using Microsoft.Win32;

namespace bms_manager.Presentation.Services.Implementations;

public sealed class FolderPickerService : IFolderPickerService
{
    public string? SelectFolder(string initialDirectory, string title)
    {
        var dialog = new OpenFolderDialog
        {
            InitialDirectory = initialDirectory,
            Title = title
        };

        return dialog.ShowDialog() == true ? dialog.FolderName : null;
    }
}