using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using bms_manager.Application.Services;
using bms_manager.Presentation.Commands;

namespace bms_manager.Presentation.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private readonly INameService _nameService;

    private string _name = string.Empty;
    private string _statusMessage = string.Empty;

    public MainViewModel(INameService nameService)
    {
        _nameService = nameService;

        SaveCommand = new RelayCommand(Save);
    }

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

    public ICommand SaveCommand { get; }

    private void Save()
    {
        try
        {
            _nameService.SaveName(Name);

            StatusMessage =
                $"Nombre guardado correctamente: {Name.Trim()}";

            Name = string.Empty;
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}