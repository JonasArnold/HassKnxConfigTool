using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class ProjectModel
  {
    public string Name { get; set; }

    ObservableCollection<LayerModel> Layers { get; set; }
  }
}
