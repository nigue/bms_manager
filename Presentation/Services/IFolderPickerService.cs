namespace bms_manager.Presentation.Services;

public interface IFolderPickerService
{
    string? SelectFolder(string initialDirectory, string title);
}
