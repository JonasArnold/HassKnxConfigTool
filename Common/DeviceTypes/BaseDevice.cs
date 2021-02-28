using Common.Mvvm;

namespace Common.DeviceTypes
{
  /// <summary>
  /// Base type for devices.
  /// </summary>
  public abstract class BaseDevice : ObservableObject, IDevice
  {
    public DeviceType Type { get; }

    public abstract string Name { get; set; }

    public BaseDevice(DeviceType type)
    {
      this.Type = type;
    }
  }
}