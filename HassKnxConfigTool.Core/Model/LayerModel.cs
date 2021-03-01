using Common.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class LayerModel : ObservableObject, IEquatable<LayerModel>, IEquatable<string>
  {
    public LayerModel()
    {
      SubLayers = new ObservableCollection<LayerModel>();
      Devices = new ObservableCollection<DeviceModel>();
      this.SubLayers.CollectionChanged += AnyCollectionChanged;
      this.Devices.CollectionChanged += AnyCollectionChanged;
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    private string name;
    public string Name
    {
      get { return this.name; }
      set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
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

    /// <summary>
    /// Shows the depth of layers.
    /// </summary>
    public int Depth { get; private set; }

    /// <summary>
    /// Unique Identifier for this layer
    /// </summary>
    public string Id { get; private set; }

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
