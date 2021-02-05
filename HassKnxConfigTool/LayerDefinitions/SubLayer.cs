using System.Collections.ObjectModel;

namespace HassKnxConfigTool.LayerDefinitions
{
  public class SubLayer : ILayer
  {
    public string Name { get; set; }
    public string ImagePath { get; set; }

    public ObservableCollection<ILayer> Members { get => null; set => throw new System.NotImplementedException("Lowest node reached"); }
  }
}
