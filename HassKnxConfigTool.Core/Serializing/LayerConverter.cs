using HassKnxConfigTool.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace HassKnxConfigTool.Core.Serializing
{
  internal class LayerConverter : JsonConverter
  {
    private static readonly Dictionary<string, ILayer> mapping = new Dictionary<string, ILayer>(StringComparer.OrdinalIgnoreCase);

    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(ILayer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      ILayer layer;
      var jsonObject = JObject.Load(reader);
      bool isLayerModel = jsonObject.ContainsKey("SubLayers");

      // deserialize values
      string parentId = jsonObject.Value<string>("ParentId");
      string thisLayerId = jsonObject.Value<string>("Id");
      int depth = jsonObject.Value<int>("Depth");

      // get parent object
      mapping.TryGetValue(parentId, out ILayer parentObj);

      if (isLayerModel)
      {
        layer = new LayerModel(parentObj, depth, thisLayerId); // if parentObj is null, this is ok.
        mapping.Add(jsonObject.Value<string>("Id"), layer); // add to mapping dictionary
      }
      else // is device model
      {
        layer = new DeviceModel(parentObj, depth, thisLayerId);// if parentObj is null, this is ok.
      }

      // populate properties
      serializer.Populate(jsonObject.CreateReader(), layer);

      return layer;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      // TODO extend to export ID here instead of in the base class
      // not supported
      throw new NotImplementedException();
    }
  }
}