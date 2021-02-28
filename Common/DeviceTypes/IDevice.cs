using System.ComponentModel;

namespace Common.DeviceTypes
{
  public interface IDevice : INotifyPropertyChanged
  {
    public DeviceType Type { get; }
    public string Name { get; set; }
  }
}
