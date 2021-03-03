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
      this.projectChangedNotifier.SelectedProjectChanged += OnSelectedProjectChanged;
    }

    private void OnSelectedProjectChanged(object sender, ProjectModel e)
    {
      if (e != null)
      {
        currentlySelectedProject = e;
        this.Layers = e.Layers;
      }
    }

    #region Commands
    private void WireCommands()
    {
      AddDeviceCommand = new RelayCommand(AddDevice);
      RemoveDeviceCommand = new RelayCommand(RemoveDevice);
      AddLayerCommand = new RelayCommand(AddLayer);
      AddSubLayerCommand = new RelayCommand(AddSubLayer);
      SelectedItemChangedCommand = new RelayCommand<object>((o) => SelectedItemChanged(o));
      RemoveLayerCommand = new RelayCommand(RemoveLayer);
    }

    public RelayCommand AddDeviceCommand { get; private set; }
    public bool CanAddDevice => this.SelectedItemIsLayerAndBelowMaxDepth && string.IsNullOrEmpty(this.NewDeviceName) == false;
    public void AddDevice()
    {
      IDevice newDevice = this.SelectedDeviceType switch
      {
        DeviceType.Light => new Light(this.NewDeviceName),// set default name
                                                          //TODO
        _ => throw new Exception($"Cannot add Device with type {this.SelectedDeviceType}."),
      };

      // create new device model
      var currentLayer = SelectedItem as LayerModel;
      DeviceModel d = new DeviceModel(currentLayer)
      {
        Name = this.NewDeviceName,
        Device = newDevice
      };
      this.NewDeviceName = "";  // remove current text

      // add newly created device to layer
      currentLayer.Devices.Add(d);
      OnPropertyChanged(nameof(this.NewDeviceName));
      OnPropertyChanged(nameof(this.Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand RemoveDeviceCommand { get; private set; }
    public bool CanRemoveDevice => this.SelectedItemIsDevice;
    public void RemoveDevice()
    {
      // remove device for max. depth of 3
      string idToRemove = ((DeviceModel)SelectedItem).Id;

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
      LayerModel l = new LayerModel(null)
      {
        Name = NewLayerName
      };
      NewLayerName = "";  // remove current text

      // add primary layer
      Layers.Add(l);
      OnPropertyChanged(nameof(NewLayerName));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand AddSubLayerCommand { get; private set; }
    public bool CanAddSubLayer => this.SelectedItemIsLayerAndBelowMaxDepth && string.IsNullOrEmpty(NewSubLayerName) == false;
    public void AddSubLayer()
    {
      // create new sub layer
      var currentLayer = this.SelectedItem as LayerModel;
      LayerModel sl = new LayerModel(currentLayer)
      {
        Name = NewSubLayerName
      };
      NewSubLayerName = "";  // remove current text

      // add newly created sub layer to sublayers
      currentLayer.SubLayers.Add(sl);
      OnPropertyChanged(nameof(NewSubLayerName));
      OnPropertyChanged(nameof(SelectedItem));
      OnPropertyChanged(nameof(Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand RemoveLayerCommand { get; private set; }
    public bool CanRemoveLayer => this.SelectedItemIsLayer;
    public void RemoveLayer()
    {
      // remove device for max. depth of 3
      string idToRemove = ((LayerModel)SelectedItem).Id;

      // use recursive helper method to remove layer
      var successfullyRemoved = LayerHelpers.FindAndRemoveLayer(idToRemove, this.Layers);
      Debug.WriteLine($"FindAndRemoveLayer Success={successfullyRemoved}");

      OnPropertyChanged(nameof(SelectedItem));
      OnPropertyChanged(nameof(Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand<object> SelectedItemChangedCommand { get; private set; }
    private void SelectedItemChanged(object arg)
    {
      if (arg is LayerModel or DeviceModel)
      {
        this.SelectedItem = arg;
      }
    }

    #endregion

    #region Properties
    private ObservableCollection<LayerModel> _layers = new ObservableCollection<LayerModel>();
    public ObservableCollection<LayerModel> Layers
    {
      get { return _layers; }
      set
      {
        _layers = value;
        OnPropertyChanged(nameof(Layers));
      }
    }

    private object _selectedItem;
    public object SelectedItem
    {
      get { return _selectedItem; }
      set
      {
        _selectedItem = value;
        this.SwitchPropertiesView(this.SelectedItem);
        OnPropertyChanged(nameof(CanAddLayer));
        OnPropertyChanged(nameof(CanAddSubLayer));
        OnPropertyChanged(nameof(CanAddDevice));
        OnPropertyChanged(nameof(CanRemoveLayer));
        OnPropertyChanged(nameof(CanRemoveDevice));
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

    private object _propertiesView;
    public object PropertiesView
    {
      get { return _propertiesView; }
      set { if (_propertiesView != value) { _propertiesView = value; OnPropertyChanged(nameof(PropertiesView)); } }
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
        this.SwitchPropertiesView(_selectedDeviceType);
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
      this.DeviceTypeValues = new List<DeviceType>
            {
                DeviceType.Light,
                DeviceType.BinarySensor,
                DeviceType.Switch,
                DeviceType.Scene
            };

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
    private void SwitchPropertiesView(object selectedItem)
    {
      if (selectedItem == null)
      {
        return; // ignore
      }

      // switch to according device view
      if (selectedItem is DeviceModel device)
      {
        this.PropertiesView = device.Device.Type switch
        {
          DeviceType.Light => (Light)device.Device,
          // TODO
          _ => throw new ImplementationException("Newly selected device type has no view assigned."),
        };

        // register for changes event
        device.Device.AnyPropertyChanged += delegate { this.SetUnsavedChanges(true); };
      }

      // switch to according layer view
      if (selectedItem is LayerModel layer)
      {
        this.PropertiesView = layer;
      }

      OnPropertyChanged(nameof(this.PropertiesView));  // notify about change
    }

    private bool SelectedItemIsDevice
    {
      get { return this.SelectedItem != null && this.SelectedItem is DeviceModel; }
    }

    private bool SelectedItemIsLayer
    {
      get { return this.SelectedItem != null && this.SelectedItem is LayerModel; }
    }

    private bool SelectedItemIsLayerAndBelowMaxDepth
    {
      get { return this.SelectedItemIsLayer && (this.SelectedItem as LayerModel).Depth < Constants.MaxLayerDepth; }
    }

    #endregion

  }
}
