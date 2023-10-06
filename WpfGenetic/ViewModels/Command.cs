using System;
using System.Windows.Input;

namespace WpfGenetic.ViewModels;

public class Command : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute;

    public event EventHandler CanExecuteChanged = delegate { };

    public Command(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged(this, null!);
    }
}