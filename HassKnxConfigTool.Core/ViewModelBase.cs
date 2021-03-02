using System.ComponentModel;

namespace HassKnxConfigTool.Core
{
  /// <summary>
  /// Base class for View Models.
  /// Note: Observable object does not work here.
  /// </summary>
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    /// <summary>
    /// Header of the view Model.
    /// </summary>
    public abstract string Header { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
  }
}
