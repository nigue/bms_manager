using System;
using System.Windows.Input;

namespace bms_manager.Presentation.Commands;

/// <summary>
/// Implementa <see cref="ICommand"/> delegando la ejecución y la comprobación
/// de disponibilidad en los métodos proporcionados.
/// </summary>
public sealed class RelayCommand : ICommand
{
    /// <summary>
    /// Acción que se ejecuta cuando se invoca el comando.
    /// </summary>
    private readonly Action _execute;

    /// <summary>
    /// Función opcional que determina si el comando puede ejecutarse.
    /// </summary>
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// Crea un comando que ejecuta una acción y, opcionalmente, comprueba
    /// si está disponible.
    /// </summary>
    /// <param name="execute">Acción que se ejecutará; no puede ser <see langword="null"/>.</param>
    /// <param name="canExecute">
    /// Función que indica si el comando puede ejecutarse. Si es <see langword="null"/>,
    /// el comando siempre está disponible.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando <paramref name="execute"/> es <see langword="null"/>.
    /// </exception>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Determina si el comando puede ejecutarse en su estado actual.
    /// </summary>
    /// <param name="parameter">Parámetro del comando; esta implementación no lo utiliza.</param>
    /// <returns>
    /// <see langword="true"/> si no se proporcionó una función de disponibilidad
    /// o si esta devuelve <see langword="true"/>; de lo contrario, <see langword="false"/>.
    /// </returns>
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    /// <summary>
    /// Ejecuta la acción asociada al comando.
    /// </summary>
    /// <param name="parameter">Parámetro del comando; esta implementación no lo utiliza.</param>
    public void Execute(object? parameter)
    {
        _execute();
    }

    /// <summary>
    /// Se produce cuando cambia la disponibilidad del comando.
    /// Suscripciones y cancelaciones se delegan a <see cref="CommandManager.RequerySuggested"/>.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}