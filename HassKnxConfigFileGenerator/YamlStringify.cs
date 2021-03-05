using Common.Attributes;
using HassKnxConfigFileGenerator.ValueParsing;
using System;
using System.Diagnostics;

namespace HassKnxConfigFileGenerator
{
  /// <summary>
  /// Creates yaml strings out of C# instances.
  /// </summary>
  internal static class YamlStringify
  {
    /// <summary>
    /// Creates a string of a generic type instance with Properties that have a PropertyNameAttribute declared.
    /// Every Type of Property does need to have a ToString method defined! 
    /// Yaml string example:
    /// - name: 'Test'
    ///   state: '2/2/23'
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    internal static string CreateInstanceString<T>(T instance)
    {
      bool firstLine = true;
      string instanceString = "- ";
      Type itemType = instance.GetType();

      Debug.WriteLine($"Properties of type: {itemType.Name}");
      foreach (var property in itemType.GetProperties())
      {
        // extract haName attribute
        string haName = AttributeHelper.GetPropertyInfoAttributeValue<T, string, PropertyNameAttribute, string>(property, attr => attr.Value);

        // if the HA name is not defined or it is not relevant for HA
        if(String.IsNullOrEmpty(haName))
        {
          // ignore this property
          continue;
        }

        // extract value of property
        var value = instance.GetType().GetProperty(property.Name).GetValue(instance, null);

        // parse to string
        string parsedValue = HassValueParser.ParseObject(value);

        if(string.IsNullOrEmpty(parsedValue) == false)  // if value is not empty
        {
          // add line to config
          if (firstLine)
          {
            instanceString += $"{haName}: '{parsedValue}'\n";  // TEST value.ToString defined?
            firstLine = false;
          }
          else
          {
            instanceString += $"  {haName}: '{parsedValue}'\n";  // two spaces in front
          }
        }
        Debug.WriteLine($"PropertyName: {property.Name},\t HA Name: {haName},\t Value: {parsedValue}");
      }
      return instanceString;
    }
  }
}
