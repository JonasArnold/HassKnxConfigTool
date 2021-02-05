using System.Collections.ObjectModel;

namespace HassKnxConfigTool.LayerDefinitions
{
  public class MainLayer : ILayer
  {
    public string Name { get; set; }

    public ObservableCollection<ILayer> Members { get; set; }

    public MainLayer()
    {
      this.Members = new ObservableCollection<ILayer>();
    }
  }
}
