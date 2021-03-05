using Common.DeviceTypes;
using Common.Exceptions;
using Common.Knx;
using System;
using System.Collections.Generic;

namespace HassKnxConfigFileGenerator.ValueParsing
{
  internal static class HassValueParser
  {
    // EXTEND_DEVICETYPES (potentially add data types)
    private static readonly Dictionary<Type, IParser> TypeParserCombination = new Dictionary<Type, IParser>()
    {
      { typeof(string), new GenericToStringParser()},
      { typeof(bool), new BooleanParser()},
      { typeof(GroupAddress), new GenericToStringParser()},
      { typeof(BinarySensorType), new FieldNameAttributeParser<BinarySensorType>()},
    };

    /// <summary>
    /// Parses any object that is supported.
    /// </summary>
    /// <param name="value">object to parse</param>
    /// <returns>string with content to write to config file</returns>
    public static string ParseObject(object value)
    {
      // ignore objects that are null
      if (value == null)
      {
        return String.Empty;
      }

      // catch types that are not supported
      if (TypeParserCombination.ContainsKey(value.GetType()) == false)
      {
        throw new ImplementationException($"Parsing value of type {value.GetType()} not supported");
      }

      string parsed = String.Empty;

      // go through every type and call their parsing method
      foreach (var type in TypeParserCombination)
      {
        // where type matches
        if (type.Key == value.GetType())
        {
          // call parsing method
          parsed = type.Value.Parse(value);
          break; // end seraching
        }
      }

      return parsed;
    }
  }
}
