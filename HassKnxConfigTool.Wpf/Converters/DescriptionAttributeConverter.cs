using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace HassKnxConfigTool.Wpf.Converters
{
  public class DescriptionAttributeConverter : IValueConverter
  {
    private static string GetEnumDescription(Enum enumObj)
    {
      if (enumObj == null)
      {
        return string.Empty;
      }
      FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

      object[] attribArray = fieldInfo.GetCustomAttributes(false);

      if (attribArray.Length == 0)
      {
        return enumObj.ToString();
      }
      else
      {
        var descAttribute = attribArray.FirstOrDefault(a => a.GetType() == typeof(DescriptionAttribute)) as DescriptionAttribute;
        return descAttribute.Description;
      }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null || (value is string stringVal && string.IsNullOrEmpty(stringVal))) // prevent exception for empty string
      {
        return null;
      }
      Enum myEnum = (Enum)value;
      if (myEnum == null)
      {
        return null;
      }
      string description = GetEnumDescription(myEnum);
      if (!string.IsNullOrEmpty(description))
      {
        return description;
      }
      return myEnum.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
