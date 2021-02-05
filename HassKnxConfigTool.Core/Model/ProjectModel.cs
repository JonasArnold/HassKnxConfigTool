using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class ProjectModel
  {
    public string Name { get; set; }

    ObservableCollection<DeviceModel> DeviceTree { get; set; }
  }
}
