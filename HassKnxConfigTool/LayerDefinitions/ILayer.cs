using System.Collections.ObjectModel;

namespace HassKnxConfigTool.LayerDefinitions
{
  public interface ILayer
  {
    public string Name { get; set; }
    ObservableCollection<ILayer> Members { get; }
  }
}
