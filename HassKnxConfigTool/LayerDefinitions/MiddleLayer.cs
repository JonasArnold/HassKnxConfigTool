using System.Collections.ObjectModel;

namespace HassKnxConfigTool.LayerDefinitions
{
  public class MiddleLayer : ILayer
  {
    public string Name { get; set; }

    public ObservableCollection<ILayer> Members { get; set; }

    public MiddleLayer()
    {
      this.Members = new ObservableCollection<ILayer>();
    }
  }
}
