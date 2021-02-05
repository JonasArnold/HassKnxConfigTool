using System.Collections.ObjectModel;
using HassKnxConfigTool.Core.Model;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class EditorViewModel : MvxViewModel
  {
    public EditorViewModel()
    {
      AddDeviceCommand = new MvxCommand(AddDevice);
    }

    public IMvxCommand AddDeviceCommand { get; set; } 

    public void AddDevice()
    {
      DeviceModel d = new DeviceModel
      {
        Name = "Test"
      };

      Devices.Add(d);
    }

    // public bool CanAddDevice => 

    private ObservableCollection<DeviceModel> _devices = new ObservableCollection<DeviceModel>();

    public ObservableCollection<DeviceModel> Devices
    {
      get { return _devices; }
      set 
      { 
        SetProperty(ref _devices, value);
        // Not necessary RaisePropertyChanged(() => Devices);
      }
    }
  }
}
