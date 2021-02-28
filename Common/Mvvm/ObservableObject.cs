using System.ComponentModel;

namespace Common.Mvvm
{
  public class ObservableObject
  {
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