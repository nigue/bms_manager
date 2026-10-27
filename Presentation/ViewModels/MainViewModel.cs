using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using bms_manager.Application.Services;
using bms_manager.Presentation.Commands;

namespace bms_manager.Presentation.ViewModels;

/// <summary>
/// Mantiene el estado de la ventana principal y expone las acciones
/// que puede ejecutar la interfaz.
/// </summary>
public sealed class MainViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Servicio de aplicación que guarda los nombres.
    /// </summary>
    private readonly INameService _nameService;

    /// <summary>
    /// Nombre que el usuario ha introducido en la interfaz.
    /// </summary>
    private string _name = string.Empty;

    /// <summary>
    /// Mensaje que informa al usuario del resultado de la operación.
    /// </summary>
    private string _statusMessage = string.Empty;

    /// <summary>
    /// Crea el ViewModel y configura el comando para guardar el nombre.
    /// </summary>
    /// <param name="nameService">Servicio que procesa y guarda el nombre.</param>
    public MainViewModel(INameService nameService)
    {
        _nameService = nameService;
        SaveCommand = new RelayCommand(Save);
    }

    /// <summary>
    /// Obtiene o establece el nombre introducido por el usuario.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value)
                return;

            _name = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Obtiene el mensaje de estado que se muestra en la interfaz.
    /// Solo el ViewModel puede modificarlo.
    /// </summary>
    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            if (_statusMessage == value)
                return;

            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Obtiene el comando que guarda el nombre actual.
    /// </summary>
    public ICommand SaveCommand { get; }

    /// <summary>
    /// Guarda el nombre actual mediante el servicio y actualiza el estado
    /// de la interfaz con el resultado.
    /// </summary>
    private void Save()
    {
        try
        {
            _nameService.SaveName(Name);
            StatusMessage = $"Nombre guardado correctamente: {Name.Trim()}";
            Name = string.Empty;
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
    }

    /// <summary>
    /// Se produce cuando cambia el valor de una propiedad del ViewModel.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Notifica a la interfaz que una propiedad ha cambiado.
    /// </summary>
    /// <param name="propertyName">
    /// Nombre de la propiedad modificada. Se completa automáticamente
    /// con el nombre del miembro que llama a este método.
    /// </param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}