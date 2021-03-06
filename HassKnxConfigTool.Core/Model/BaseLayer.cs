using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace HassKnxConfigTool.Core.Model
{
  public abstract class BaseLayer : INotifyPropertyChanged, ILayer
  {
    public BaseLayer(ILayer parentLayer)
    {
      this.Depth = CalcLayerDepth(parentLayer);
      this.Parent = parentLayer;
      this.ParentId = parentLayer == null ? "" : parentLayer.Id; // empty string if no parent layer
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    /// <summary>
    /// Alternative contructor used for deserialization.
    /// To fill up correctly deserialized ID's and depth.
    /// </summary>
    /// <param name="parentLayer">parent layer</param>
    /// <param name="depth">deserialized depth</param>
    /// <param name="myId">deserialized id of this instance</param>
    public BaseLayer(ILayer parentLayer, int depth, string myId)
    {
      this.Depth = depth; // from deserialized
      this.Parent = parentLayer; // from deserialized
      this.ParentId = parentLayer == null ? "" : parentLayer.Id; // empty string if no parent layer
      Id = myId;  // from deserialized
    }

    /// <summary>
    /// Calculates the layer depth of this layer according to the parent layer.
    /// </summary>
    /// <param name="parentLayer">parent layer</param>
    /// <returns>depth calculated</returns>
    private static int CalcLayerDepth(ILayer parentLayer)
    {
      // if top layer => return 0
      if (parentLayer == null)
      {
        return 0;
      }
      else  // otherwise one deeper than the previous
      {
        return parentLayer.Depth + 1;
      }
    }

    #region Tree logic
    /// <summary>
    /// Is Expanded Property indicates whether the tree node is expanded.
    /// </summary>
    public abstract bool IsExpanded { get; set; }
   
    private bool isSelected;
    /// <summary>
    /// Identifies if this layer is selected.
    /// </summary>
    [JsonIgnore]
    public bool IsSelected
    {
      get { return this.isSelected; }
      set
      {
        this.isSelected = value;
        OnPropertyChanged(nameof(this.IsSelected));
        if (this.isSelected)
        {
          SelectedItem = this;  // now this class is selected
        }
      }
    }

    /// <summary>
    /// Gets raised to inform other classes about a change of the selected item.
    /// </summary>
    public static event EventHandler<object> SelectedItemChanged;

    private static object _selectedItem = null;
    /// <summary>
    /// Static getter for the selected item.
    /// Can be used everywhere to get the currently selected item.
    /// </summary>
    [JsonIgnore]
    public static object SelectedItem
    {
      get { return _selectedItem; }
      private set
      {
        if (_selectedItem != value)
        {
          _selectedItem = value;
          SelectedItemChanged?.Invoke(null, _selectedItem);
        }
      }
    }

    /// <summary>
    /// Used to reset the selected item to null. 
    /// </summary>
    public static void ResetSelectedItem()
    {
      SelectedItem = null;
    }
    #endregion

    #region ILayer members
    private string name;
    public string Name
    {
      get { return this.name; }
      set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
    }

    public int Depth { get; set; }
    public string Id { get; set; }

    private ILayer parent;
    [JsonIgnore]
    public ILayer Parent
    {
      get { return this.parent; }
      private set { this.parent = value; }
    }

    private string parentID;
    /// <summary>
    /// Used to deserialize.
    /// String is empty if there is no parent
    /// </summary>
    public string ParentId
    {
      get { return this.parentID; }
      private set { this.parentID = value; }
    }

    #endregion

    #region INotifyPropertyChanged members
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
    #endregion
  }
}
