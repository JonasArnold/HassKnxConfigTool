using Common.Mvvm;
using System;

namespace Common.DeviceTypes
{
  /// <summary>
  /// Base type for devices.
  /// </summary>
  public abstract class BaseDevice : ObservableObject, IDevice
  {
    public DeviceType Type { get; }

    public abstract string Name { get; set; }

    public event EventHandler AnyPropertyChanged;

    public BaseDevice(DeviceType type)
    {
      this.Type = type;
      base.PropertyChanged += BaseDevice_PropertyChanged;
    }


    /// <summary>
    /// Is called whenever a property of the device is updated via <see cref="ObservableObject"/>.
    /// </summary>
    private void BaseDevice_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      OnAnyPropertyChanged();
    }

    /// <summary>
    /// Method to call whenever any property inside the device was changed.
    /// </summary>
    internal void OnAnyPropertyChanged()
    {
      this.AnyPropertyChanged?.Invoke(this, null);
    }

  }
}