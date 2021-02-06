using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class LayerModel : IEquatable<LayerModel>, IEquatable<string>
  {
    public LayerModel()
    {
      SubLayers = new ObservableCollection<LayerModel>();
      Devices = new ObservableCollection<DeviceModel>();
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    public string Name { get; set; }

    public ObservableCollection<LayerModel> SubLayers { get; private set; }

    public ObservableCollection<DeviceModel> Devices { get; private set; }

    /// <summary>
    /// Shows the depth of layers.
    /// </summary>
    public int Depth { get; private set; }

    /// <summary>
    /// Unique Identifier for this layer
    /// </summary>
    public string Id { get; private set; }

    #region IEquatable
    public bool Equals(LayerModel other)
    {
      return this.Id == other.Id;
    }

    public bool Equals(string id)
    {
      return this.Id == id;
    }
    #endregion
  }
}
