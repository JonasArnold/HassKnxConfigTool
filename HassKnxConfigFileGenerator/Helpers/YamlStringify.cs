﻿using HassKnxConfigFileGenerator.DeviceTypeDefinitions;
using System;

namespace HassKnxConfigFileGenerator.Helpers
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

      Console.WriteLine($"Properties of type: {itemType.Name}");
      foreach (var property in itemType.GetProperties())
      {
        string haName = AttributeHelper.GetPropertyInfoAttributeValue<T, string, PropertyNameAttribute, string>(property, attr => attr.Value);
        var value = instance.GetType().GetProperty(property.Name).GetValue(instance, null);
        if(string.IsNullOrEmpty(value?.ToString()) == false)  // if value is not empty
        {
          // add line to config
          if (firstLine)
          {
            instanceString += $"{haName}: '{value}'\n";  // TEST value.ToString defined?
            firstLine = false;
          }
          else
          {
            instanceString += $"  {haName}: '{value}'\n";  // two spaces in front
          }
        }
        Console.WriteLine($"PropertyName: {property.Name},\t HA Name: {haName},\t Value: {value}");
      }
      return instanceString;
    }
  }
}
