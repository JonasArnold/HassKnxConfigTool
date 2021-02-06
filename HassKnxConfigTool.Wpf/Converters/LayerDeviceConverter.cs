using Common.Structure;
using HassKnxConfigTool.Core.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HassKnxConfigTool.Wpf.Converters
{
  public class LayerDeviceConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      ObservableCollection<LayerModel> layers = (ObservableCollection<LayerModel>)values[0];
      ObservableCollection<DeviceModel> devices = (ObservableCollection<DeviceModel>)values[1];
      List<object> items = new List<object>();

      items.AddRange(layers);
      items.AddRange(devices);

      return items;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
