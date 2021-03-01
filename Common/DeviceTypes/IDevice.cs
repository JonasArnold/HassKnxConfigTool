using System;
using System.ComponentModel;

namespace Common.DeviceTypes
{
  public interface IDevice : INotifyPropertyChanged
  {
    /// <summary>
    /// Type of device.
    /// </summary>
    public DeviceType Type { get; }

    /// <summary>
    /// Name of the device.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Event that gets raised whenever a property inside the device was changed.
    /// </summary>
    public event EventHandler AnyPropertyChanged;
  }
}
