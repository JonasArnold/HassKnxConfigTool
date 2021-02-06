using System.ComponentModel;

namespace Common.DeviceTypes
{
  public enum DeviceTypes
  {
    [Description("Light")]
    Light,

    [Description("Switch")]
    Switch,

    [Description("Binary Sensor")]
    BinarySensor,

    [Description("Scene")]
    Scene
  }
}
