using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Common.DeviceTypes;
using Common.Mvvm;
using HassKnxConfigTool.Core.Model;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class EditorViewModel : ViewModelBase
  {
    public string Header => "Editor";
    private readonly IUiService uiService;
    private readonly IProjectChangedNotifier projectChangedNotifier;
    private ProjectModel currentlySelectedProject;
    private const int MaxLayerDepth = 3; // e.g. Main, Middle, Sub, Devices

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
    public bool CanAddDevice => SelectedItem != null && SelectedItem is LayerModel && string.IsNullOrEmpty(this.NewDeviceName) == false;
    public void AddDevice()
    {
      IDevice newDevice;
      switch (this.SelectedDeviceType)
      {
        case DeviceType.Light:
          newDevice = new Light(this.NewDeviceName); // set default name
          break;
        case DeviceType.Switch:  //TODO
        case DeviceType.BinarySensor:
        case DeviceType.Scene:
        case DeviceType.None:
        default:
          throw new Exception($"Cannot add Device with type {this.SelectedDeviceType}.");
      }
      DeviceModel d = new DeviceModel
      {
        Name = this.NewDeviceName,
        Device = newDevice
      };
      this.NewDeviceName = "";  // remove current text

      ((LayerModel)SelectedItem).Devices.Add(d);
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
      bool removed = false;
      foreach (var subItem in Layers)
      {
        if (removed == false)
        {
          removed &= subItem.Devices.Remove(subItem.Devices.FirstOrDefault(d => d.Id == idToRemove));
          foreach (var subSubItem in subItem.SubLayers)
          {
            if (removed == false)
            {
              removed &= subSubItem.Devices.Remove(subSubItem.Devices.FirstOrDefault(d => d.Id == idToRemove));
              foreach (var subSubSubItem in subSubItem.SubLayers)
              {
                removed &= subSubSubItem.Devices.Remove(subSubSubItem.Devices.FirstOrDefault(d => d.Id == idToRemove));
              }
            }
          }
        }
      }

      this.SetUnsavedChanges(true);
    }

    public RelayCommand AddLayerCommand { get; private set; }
    public bool CanAddLayer => string.IsNullOrEmpty(NewLayerName) == false;
    public void AddLayer()
    {
      LayerModel l = new LayerModel
      {
        Name = NewLayerName
      };
      NewLayerName = "";  // remove current text

      Layers.Add(l);
      OnPropertyChanged(nameof(NewLayerName));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand AddSubLayerCommand { get; private set; }
    public bool CanAddSubLayer => SelectedItem != null && SelectedItem is LayerModel && string.IsNullOrEmpty(NewSubLayerName) == false;
    public void AddSubLayer()
    {
      LayerModel sl = new LayerModel
      {
        Name = NewSubLayerName
      };
      NewSubLayerName = "";  // remove current text

      ((LayerModel)SelectedItem).SubLayers.Add(sl);
      OnPropertyChanged(nameof(NewSubLayerName));
      OnPropertyChanged(nameof(SelectedItem));
      OnPropertyChanged(nameof(Layers));
      this.SetUnsavedChanges(true);
    }

    public RelayCommand RemoveLayerCommand { get; private set; }
    public bool CanRemoveLayer => SelectedItem != null && SelectedItem is LayerModel;
    public void RemoveLayer()
    {
      // remove device for max. depth of 3
      string idToRemove = ((LayerModel)SelectedItem).Id;
      bool removed = false;

      removed &= Layers.Remove(Layers.FirstOrDefault(d => d.Id == idToRemove));
      if (removed == false)
      {
        foreach (var layer in Layers)
        {
          removed &= Layers.Remove(layer.SubLayers.FirstOrDefault(d => d.Id == idToRemove));
          if (removed == false)
          {
            removed &= layer.SubLayers.Remove(layer.SubLayers.FirstOrDefault(d => d.Id == idToRemove));
            foreach (var subLayer in layer.SubLayers)
            {
              if (removed == false)
              {
                removed &= subLayer.SubLayers.Remove(subLayer.SubLayers.FirstOrDefault(d => d.Id == idToRemove));
                foreach (var subSubLayer in subLayer.SubLayers)
                {
                  removed &= subSubLayer.SubLayers.Remove(subSubLayer.SubLayers.FirstOrDefault(d => d.Id == idToRemove));
                }
              }
            }
          }
        }

        OnPropertyChanged(nameof(SelectedItem));
        OnPropertyChanged(nameof(Layers));
        this.SetUnsavedChanges(true);
      }
    }


    public RelayCommand<object> SelectedItemChangedCommand { get; private set; }
    private void SelectedItemChanged(object arg)
    {
      if (arg is LayerModel layer)
      {
        SelectedItem = layer;
      }
      if (arg is DeviceModel device)
      {
        SelectedItem = device;
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
        switch (device.Device.Type)
        {
          case DeviceType.Light:
            this.PropertiesView = (Light)device.Device;
            break;
          case DeviceType.Switch: // TODO
          case DeviceType.BinarySensor:
          case DeviceType.Scene:
          case DeviceType.None:
          default:
            throw new ArgumentOutOfRangeException("Newly selected device type has no view assigned.");
        }

        // register for changes event
        device.Device.AnyPropertyChanged += delegate { this.SetUnsavedChanges(true); }; 
      }

      // switch to according layer view
      if (selectedItem is LayerModel layer)
      {
        this.PropertiesView = layer;
      }
    }

    private bool SelectedItemIsDevice
    {
      get { return SelectedItem != null && SelectedItem is DeviceModel; }
    }

    #endregion

  }
}
