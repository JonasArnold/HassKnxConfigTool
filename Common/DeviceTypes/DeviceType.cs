using System.ComponentModel;

namespace Common.DeviceTypes
{
  public enum DeviceType
  {
    [Description("")]
    None, 

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
