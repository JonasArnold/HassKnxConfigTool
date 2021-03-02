using System.ComponentModel;

namespace Common.Mvvm
{
  public class ObservableObject
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
  }
}