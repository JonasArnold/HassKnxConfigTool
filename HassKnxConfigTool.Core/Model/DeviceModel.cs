using Common.Structure;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class DeviceModel : ILayer
  {
    public string Name { get; set; }

    public ObservableCollection<ILayer> Members { get; }
  }
}
