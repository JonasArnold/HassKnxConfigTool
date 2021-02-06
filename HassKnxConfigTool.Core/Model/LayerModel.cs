using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class LayerModel
  {
    public LayerModel()
    {
      SubLayers = new ObservableCollection<LayerModel>();
      Devices = new ObservableCollection<DeviceModel>();
    }

    public string Name { get; set; }

    public ObservableCollection<LayerModel> SubLayers { get; private set; }

    public ObservableCollection<DeviceModel> Devices { get; private set; }

  }
}
