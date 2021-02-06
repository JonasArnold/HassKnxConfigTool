using Common.DeviceTypes;

namespace HassKnxConfigTool.Core.Model
{
  public class DeviceModel
  {
    public string Name { get; set; }

    public IDevice Device { get; }
  }
}
