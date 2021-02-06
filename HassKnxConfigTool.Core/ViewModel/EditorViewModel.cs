using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Mvvm;
using HassKnxConfigTool.Core.Model;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class EditorViewModel : ViewModelBase
  {
    public string Header => "Editor";
    private readonly IUiService uiService;

    public EditorViewModel(IUiService uiService)
    {
      WireCommands();
      this.uiService = uiService;
    }

    private void WireCommands()
    {
      AddDeviceCommand = new RelayCommand(AddDevice);
      AddLayerCommand = new RelayCommand(AddLayer);
      AddSubLayerCommand = new RelayCommand(AddSubLayer);
      SelectedItemChangedCommand = new RelayCommand<object>((o) => SelectedItemChanged(o));
    }

    public RelayCommand AddDeviceCommand
    {
      get;
      private set;
    }

    public RelayCommand AddLayerCommand
    {
      get;
      private set;
    }

    public RelayCommand AddSubLayerCommand
    {
      get;
      private set;
    }
    public RelayCommand<object> SelectedItemChangedCommand
    {
      get;
      private set;
    }

    public void AddDevice()
    {
      DeviceModel d = new DeviceModel
      {
        Name = NewDeviceName
      };

      SelectedLayer.Devices.Add(d);
    }

    public void AddLayer()
    {
      LayerModel l = new LayerModel
      {
        Name = NewLayerName
      };

      Layers.Add(l);
    }

    public void AddSubLayer()
    {
      LayerModel sl = new LayerModel
      {
        Name = NewSubLayerName
      };

      (SelectedLayer).SubLayers.Add(sl);
      OnPropertyChanged(nameof(SelectedLayer));
      OnPropertyChanged(nameof(Layers));
    }


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


    private ObservableCollection<DeviceModel> _devices = new ObservableCollection<DeviceModel>();

    public ObservableCollection<DeviceModel> Devices
    {
      get { return _devices; }
      set 
      {
        _devices = value;
        OnPropertyChanged(nameof(Devices));
      }
    }

    private LayerModel _selectedLayer;

    public LayerModel SelectedLayer
    {
      get { return _selectedLayer; }
      set 
      { 
        _selectedLayer = value;
        OnPropertyChanged(nameof(CanAddSubLayer));
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


    public bool CanAddLayer => string.IsNullOrEmpty(NewLayerName) == false;

    public bool CanAddDevice => string.IsNullOrEmpty(NewDeviceName) == false;

    public bool CanAddSubLayer => SelectedLayer != null && SelectedLayer is LayerModel;

    private void SelectedItemChanged(object arg)
    {
      if(arg is LayerModel layer)
      {
        SelectedLayer = layer;
      }
    }
  }
}
