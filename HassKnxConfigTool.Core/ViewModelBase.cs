using System.ComponentModel;

namespace HassKnxConfigTool.Core
{
  public abstract class ViewModelBase : INotifyPropertyChanged
  {
    /// <summary>
    /// Header of the view Model.
    /// </summary>
    string Header { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propName));
      }
    }
  }
}
