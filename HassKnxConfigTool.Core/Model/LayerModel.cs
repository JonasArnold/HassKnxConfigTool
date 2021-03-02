using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  [Serializable]
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  public class LayerModel : BaseLayer, IEquatable<LayerModel>, IEquatable<string>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  {
    public LayerModel(ILayer parentLayer)
      :base(parentLayer)
    {
      SubLayers = new ObservableCollection<LayerModel>();
      Devices = new ObservableCollection<DeviceModel>();
      this.SubLayers.CollectionChanged += AnyCollectionChanged;
      this.Devices.CollectionChanged += AnyCollectionChanged;
    }

    private ObservableCollection<LayerModel> subLayers;
    /// <summary>
    /// List of Sub Layers below this Layer.
    /// </summary>
    public ObservableCollection<LayerModel> SubLayers
    {
      get { return this.subLayers; }
      set { this.subLayers = value; OnPropertyChanged(nameof(this.SubLayers)); }
    }

    private ObservableCollection<DeviceModel> devices;
    /// <summary>
    /// List of devices that are on this layer.
    /// </summary>
    public ObservableCollection<DeviceModel> Devices
    {
      get { return this.devices; }
      set { this.devices = value; OnPropertyChanged(nameof(this.Devices)); }
    }

    private int subItemsCount;
    /// <summary>
    /// Displays the number of items available below this layer.
    /// </summary>
    [JsonIgnore]
    public int SubItemsCount
    {
      get { return this.subItemsCount; }
      set { this.subItemsCount = value; OnPropertyChanged(nameof(this.SubItemsCount)); } // TODO UPDATE ALL LAYERS
    }

    /// <summary>
    /// Handles changes in the collections. => Keeps count of devices and sublayers in UI updated.
    /// </summary>
    private void AnyCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      int sum = 0;
      if (this.SubLayers != null) sum += this.SubLayers.Count;
      if (this.Devices != null) sum += this.Devices.Count;
      this.SubItemsCount = sum;
    }

    #region IEquatable members
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
