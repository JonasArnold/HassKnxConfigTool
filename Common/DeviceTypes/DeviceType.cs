using Common.Attributes;

namespace Common.DeviceTypes
{
  public enum DeviceType
  {
    [DisplayName(" ")]
    None, 

    [DisplayName("Light")]
    Light,

    [DisplayName("Switch")]
    Switch,

    [DisplayName("Binary Sensor")]
    BinarySensor,

    [DisplayName("Scene")]
    Scene,

    [DisplayName("Cover")]
    Cover
  }
}
