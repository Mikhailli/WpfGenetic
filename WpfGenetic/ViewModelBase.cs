using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace WpfGenetic;

public class ViewModelBase : INotifyPropertyChanged
{
    protected Dispatcher CurrentDispatcher = Application.Current.Dispatcher;
    
    public event PropertyChangedEventHandler? PropertyChanged = delegate {  };

    protected void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    protected void RaisePropertyChangedInCurrentDispatcher(string propertyName)
    {
        CurrentDispatcher.Invoke(() => RaisePropertyChanged(propertyName));
    }
}