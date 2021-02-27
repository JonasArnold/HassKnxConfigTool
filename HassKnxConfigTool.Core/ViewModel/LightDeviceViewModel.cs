using Common.Knx;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class LightDeviceViewModel : IDeviceViewModel
  {
    public GroupAddress Address { get; set; }

    public LightDeviceViewModel()
    {

    }
  }
}
