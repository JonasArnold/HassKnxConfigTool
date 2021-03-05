using Common.DeviceTypes;
using HassKnxConfigTool.Core.Model;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace HassKnxConfigTool.Wpf.Converters
{
  /// <summary>
  /// Should return string like the following: 
  /// Source="/HassKnxConfigTool.Wpf;component/Assets/device_hub.png"
  /// </summary>
  public class DeviceImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string path = "/HassKnxConfigTool.Wpf;component/Assets/"; // default device_hub.png

      if (value is DeviceType deviceType)
      {
        path += deviceType switch
        {
          DeviceType.Light => "lightbulb.png",
          DeviceType.Switch => "toggle.png",
          DeviceType.BinarySensor => "power-off.png",
          DeviceType.Scene => "palette.png",
          _ => "device_hub.png",  // default device image
        };
      }
      else
      {
        // default device image
        path += "device_hub.png";
      }

      var uriSource = new Uri(path, UriKind.Relative);
      return new BitmapImage(uriSource);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
