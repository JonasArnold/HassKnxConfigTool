using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Common.DeviceTypes;
using Common.Mvvm;
using HassKnxConfigTool.Core.Model;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class EditorViewModel : ViewModelBase
  {
    #region ViewModelBase members
    public override string Header => "Editor";
    #endregion

    private readonly IUiService uiService;
    private readonly IProjectChangedNotifier projectChangedNotifier;
    private ProjectModel currentlySelectedProject;

    public EditorViewModel(IUiService uiService, IProjectChangedNotifier projectChangedNotifier)
    {
      WireCommands();
      this.InitEnumValues();
      this.uiService = uiService;
      this.projectChangedNotifier = projectChangedNotifier;

      // event registration
      this.projectChangedNotifier.SelectedProjectChanged += OnSelectedProjectChanged;
      BaseLayer.SelectedItemChanged += OnSelectedItemChanged; 
    }

    private void OnSelectedProjectChanged(object sender, ProjectModel e)
    {
      if (e != null)
      {
        currentlySelectedProject = e;
        this.Layers = e.Layers;
        BaseLayer.ResetSelectedItem();
      }
    }

    private void OnSelectedItemChanged(object sender, object newItem)
    {
      this.SwitchPropertiesViews(newItem);
      OnPropertyChanged(nameof(Layers));
      OnPropertyChanged(nameof(CanCollapse));
      OnPropertyChanged(nameof(CanExpand));
      OnPropertyChanged(nameof(CanAddLayer));
      OnPropertyChanged(nameof(CanAddSubLayer));
      OnPropertyChanged(nameof(CanAddDevice));
      OnPropertyChanged(nameof(CanRemoveLayer));
      OnPropertyChanged(nameof(CanRemoveDevice));
    }

    #region Commands
    private void WireCommands()
    {
      // Business Commands
      AddDeviceCommand = new RelayCommand(AddDevice);
      RemoveDeviceCommand = new RelayCommand(RemoveDevice);
      AddLayerCommand = new RelayCommand(AddLayer);
      AddSubLayerCommand = new RelayCommand(AddSubLayer);
      RemoveLayerCommand = new RelayCommand(RemoveLayer);

      // Tree View Control
      ExpandCommand = new RelayCommand(() => Expand(BaseLayer.SelectedItem as LayerModel));
      CollapseCommand = new RelayCommand(() => Collapse(BaseLayer.SelectedItem as LayerModel));
    }

    #region Business Commands
    public RelayCommand AddDeviceCommand { get; private set; }
    public bool CanAddDevice => SelectedItemIsLayerAndBelowMaxDepth && string.IsNullOrEmpty(this.NewDeviceName) == false;
    public void AddDevice()
    {
      IDevice newDevice = this.SelectedDeviceType switch  // EXTEND_DEVICETYPES
      {
        // instantiate new device, always set default name
        DeviceType.Light => new Common.DeviceTypes.Light(this.NewDeviceName),
        DeviceType.Switch => new Common.DeviceTypes.Switch(this.NewDeviceName),
        DeviceType.BinarySensor => new Common.DeviceTypes.BinarySensor(this.NewDeviceName),
        DeviceType.Scene => new Common.DeviceTypes.Scene(this.NewDeviceName),
        DeviceType.Cover => new Common.DeviceTypes.Cover(this.NewDeviceName),
        _ => throw new ImplementationException($"Cannot add Device with type {this.SelectedDeviceType}."),
      };

      // create new device model
      var currentLayer = BaseLayer.SelectedItem as LayerModel;
      var d = new DeviceModel(currentLayer)
      {
        Name = this.NewDeviceName,
        Device = newDevice,
        IsSelected = true  // select newly added device
      };
      this.NewDeviceName = "";  // remove current text

      // add newly created device to layer
      currentLayer.Devices.Add(d);
      OnPropertyChanged(nameof(this.NewDeviceName));
      OnPropertyChanged(nameof(this.Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand RemoveDeviceCommand { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is bound to UI")]
    public bool CanRemoveDevice => SelectedItemIsDevice;
    public void RemoveDevice()
    {
      // remove device for max. depth of 3
      string idToRemove = ((DeviceModel)BaseLayer.SelectedItem).Id;

      // use recursive helper method to remove layer
      var successfullyRemoved = LayerHelpers.FindAndRemoveDevice(idToRemove, this.Layers);
      Debug.WriteLine($"FindAndRemoveDevice Success={successfullyRemoved}");

      this.SetUnsavedChanges(true);
    }

    public RelayCommand AddLayerCommand { get; private set; }
    public bool CanAddLayer => string.IsNullOrEmpty(NewLayerName) == false;
    public void AddLayer()
    {
      // create new primary layer (null => primary layer)
      var l = new LayerModel(null)
      {
        Name = NewLayerName,
        IsExpanded = true, // auto expand newly added layer
        IsSelected = true  // select newly added layer
      };
      NewLayerName = "";  // remove current text

      // add primary layer
      Layers.Add(l);
      OnPropertyChanged(nameof(NewLayerName));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand AddSubLayerCommand { get; private set; }
    public bool CanAddSubLayer => SelectedItemIsLayerAndBelowMaxDepth && string.IsNullOrEmpty(NewSubLayerName) == false;
    public void AddSubLayer()
    {
      // create new sub layer
      var currentLayer = BaseLayer.SelectedItem as LayerModel;
      var sl = new LayerModel(currentLayer)
      {
        Name = NewSubLayerName,
        IsExpanded = true, // auto expand newly added layer
        IsSelected = true  // select newly added layer
      };
      NewSubLayerName = "";  // remove current text

      // add newly created sub layer to sublayers
      currentLayer.SubLayers.Add(sl);
      OnPropertyChanged(nameof(NewSubLayerName));
      OnPropertyChanged(nameof(BaseLayer.SelectedItem));
      OnPropertyChanged(nameof(Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand RemoveLayerCommand { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is bound to UI")]
    public bool CanRemoveLayer => SelectedItemIsLayer;
    public void RemoveLayer()
    {
      // remove device for max. depth of 3
      string idToRemove = ((LayerModel)BaseLayer.SelectedItem).Id;

      // use recursive helper method to remove layer
      var successfullyRemoved = LayerHelpers.FindAndRemoveLayer(idToRemove, this.Layers);
      Debug.WriteLine($"FindAndRemoveLayer Success={successfullyRemoved}");

      OnPropertyChanged(nameof(Layers));
      this.SetUnsavedChanges(true);
    }
    #endregion

    #region Tree View Control Commands  
    public RelayCommand ExpandCommand { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is bound to UI")]
    public bool CanExpand => SelectedItemIsLayer;
    public void Expand(LayerModel layer)
    {
      layer.IsExpanded = true;
      foreach (var item in layer.SubLayers)
      {
        Expand(item);
      }
    }

    public RelayCommand CollapseCommand { get; private set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Is bound to UI")]
    public bool CanCollapse => SelectedItemIsLayer;
    public void Collapse(LayerModel layer)
    {
      layer.IsExpanded = false;
      foreach (var item in layer.SubLayers)
      {
        Collapse(item);
      }
    }
    #endregion

    #endregion

    #region Properties
    private ObservableCollection<LayerModel> _layers = new();
    public ObservableCollection<LayerModel> Layers
    {
      get { return _layers; }
      set
      {
        _layers = value;
        OnPropertyChanged(nameof(Layers)); 
      }
    }

    private string _newDeviceName;
    public string NewDeviceName
    {
      get { return _newDeviceName; }
      set
      {
        _newDeviceName = value;
        OnPropertyChanged(nameof(NewDeviceName));
        OnPropertyChanged(nameof(CanAddDevice));
      }
    }

    private object _devicePropertiesView;
    public object DevicePropertiesView
    {
      get { return _devicePropertiesView; }
      set { if (_devicePropertiesView != value) { _devicePropertiesView = value; OnPropertyChanged(nameof(DevicePropertiesView)); } }
    }

    private ILayer _layerPropertiesView;
    public ILayer LayerPropertiesView
    {
      get { return _layerPropertiesView; }
      set { if (_layerPropertiesView != value) { _layerPropertiesView = value; OnPropertyChanged(nameof(LayerPropertiesView)); } }
    }

    private IDevice _newDeviceInstance;
    public IDevice NewDeviceInstance
    {
      get { return _newDeviceInstance; }
      set
      {
        _newDeviceInstance = value;
        OnPropertyChanged(nameof(NewDeviceInstance));
      }
    }

    private string _newLayerName;
    public string NewLayerName
    {
      get { return _newLayerName; }
      set
      {
        _newLayerName = value;
        OnPropertyChanged(nameof(NewLayerName));
        OnPropertyChanged(nameof(CanAddLayer));
      }
    }

    private string _newSubLayerName;
    public string NewSubLayerName
    {
      get { return _newSubLayerName; }
      set
      {
        _newSubLayerName = value;
        OnPropertyChanged(nameof(NewSubLayerName));
        OnPropertyChanged(nameof(CanAddSubLayer));
      }
    }

    private List<DeviceType> _deviceTypeValues;
    public List<DeviceType> DeviceTypeValues
    {
      get { return _deviceTypeValues; }
      set
      {
        _deviceTypeValues = value;
        OnPropertyChanged(nameof(DeviceTypeValues));
      }
    }

    private DeviceType _selectedDeviceType;
    public DeviceType SelectedDeviceType
    {
      get { return _selectedDeviceType; }
      set
      {
        _selectedDeviceType = value;
        OnPropertyChanged(nameof(SelectedDeviceType));
        this.SwitchPropertiesViews(_selectedDeviceType);
      }
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Initializes the Enum <see cref="DeviceTypeValues"/>. 
    /// Selects an item for <see cref="SelectedDeviceType"/>.
    /// </summary>
    private void InitEnumValues()
    {
      // parse enum
      this.DeviceTypeValues = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().ToList();

      // remove none field, since the user shall not be able to select this
      this.DeviceTypeValues.Remove(DeviceType.None);

      // select first device type
      if (this.DeviceTypeValues.Any())
      {
        this.SelectedDeviceType = this.DeviceTypeValues.First();
      }
    }

    /// <summary>
    /// Sets the flag for unsaved changes.
    /// </summary>
    /// <param name="hasUnsavedChanges">true if there are unsaved changes.</param>
    private void SetUnsavedChanges(bool hasUnsavedChanges)
    {
      if (this.currentlySelectedProject != null)
      {
        this.currentlySelectedProject.HasUnsavedChanges = hasUnsavedChanges;
      }
      this.uiService.UpdateUnsavedChangesDisplay(hasUnsavedChanges);  // property changed event
    }

    /// <summary>
    /// Assigns the selected Item to the properties view.
    /// </summary>
    /// <param name="selectedItem">selected item to edit with properties view</param>
    private void SwitchPropertiesViews(object selectedItem)
    {
      if (selectedItem == null)
      {
        this.LayerPropertiesView = null;
        this.DevicePropertiesView = new EmptyViewModel();
        return; // ignore rest
      }

      // switch to according layer view
      if (selectedItem is LayerModel layer)
      {
        this.LayerPropertiesView = layer;
        this.DevicePropertiesView = new EmptyViewModel();
      }

      // switch to according device view
      else if (selectedItem is DeviceModel device)
      {
        this.LayerPropertiesView = device;
        this.DevicePropertiesView = device.Device.Type switch  // EXTEND_DEVICETYPES
        {
          // instantiate view
          DeviceType.Light => (Common.DeviceTypes.Light)device.Device,
          DeviceType.Switch => (Common.DeviceTypes.Switch)device.Device,
          DeviceType.BinarySensor => (Common.DeviceTypes.BinarySensor)device.Device,
          DeviceType.Scene => (Common.DeviceTypes.Scene)device.Device,
          DeviceType.Cover => (Common.DeviceTypes.Cover)device.Device,
          _ => throw new ImplementationException("Newly selected device type has no view assigned."),
        };

        // register for changes event
        device.Device.AnyPropertyChanged += delegate { this.SetUnsavedChanges(true); };
      }

      OnPropertyChanged(nameof(this.LayerPropertiesView));  // notify about change
      OnPropertyChanged(nameof(this.DevicePropertiesView));
    }

    private static bool SelectedItemIsDevice
    {
      get { return BaseLayer.SelectedItem != null && BaseLayer.SelectedItem is DeviceModel; }
    }

    private static bool SelectedItemIsLayer
    {
      get { return BaseLayer.SelectedItem != null && BaseLayer.SelectedItem is LayerModel; }
    }

    private static bool SelectedItemIsLayerAndBelowMaxDepth
    {
      get { return SelectedItemIsLayer && (BaseLayer.SelectedItem as LayerModel).Depth < Constants.MaxLayerDepth; }
    }

    #endregion

  }
}
