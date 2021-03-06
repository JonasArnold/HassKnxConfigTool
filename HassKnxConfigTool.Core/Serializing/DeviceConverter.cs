using Common.DeviceTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HassKnxConfigTool.Core.Serializing
{
  /// <summary>
  /// Custom JsonConverter to enable deserialization of Interface IDevice.
  /// => Read out of type of the device.
  /// Built according to this guide: https://skrift.io/issues/bulletproof-interface-deserialization-in-jsonnet/
  /// </summary>
  internal class DeviceConverter : JsonConverter
  {
    public override bool CanWrite => false;
    public override bool CanRead => true;

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(IDevice);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var jsonObject = JObject.Load(reader);
      var device = default(IDevice);
      switch (jsonObject["Type"].Value<int>())
      {
        case (int)DeviceType.Light:
          device = new Light();
          break;

        case (int)DeviceType.BinarySensor:
          device = new BinarySensor();
          break;

        case (int)DeviceType.Scene:
          // TODO
          break;

        case (int)DeviceType.Switch:
          device = new Switch();
          break;

        default:
        case (int)DeviceType.None:
          throw new JsonReaderException($"Cannot deserialize device type {jsonObject["Type"].Value<int>()}.");
      }

      serializer.Populate(jsonObject.CreateReader(), device);
      return device;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      throw new InvalidOperationException("Use default serialization.");
    }
  }
}