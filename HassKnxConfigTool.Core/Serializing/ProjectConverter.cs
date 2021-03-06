using HassKnxConfigTool.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace HassKnxConfigTool.Core.Serializing
{
  /// <summary>
  /// Project converter.
  /// Makes sure that no Project is deserialized which is not supported by this software.
  /// </summary>
  internal class ProjectConverter : JsonConverter
  {
    public override bool CanRead => true;
    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(ProjectModel);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      var jsonObject = JObject.Load(reader);
      string projectName = jsonObject.ContainsKey("Name") ? jsonObject["Name"].Value<string>() : ""; // extract project name if found

      // if the DataVersion cannot be found in the file => cannot import
      if(jsonObject.ContainsKey("DataVersion") == false)
      {
        throw new JsonReaderException($"Cannot deserialize project <{projectName}>, DataVersion not found.");
      }

      // if the DataVersion of the file is bigger than this software's version => can probably not read
      var dataVersionFile = jsonObject["DataVersion"].Value<int>();
      if (dataVersionFile > Constants.DataVersion)
      {
        throw new JsonReaderException($"Cannot deserialize project <{projectName}>, Software version to low to read DataVersion {dataVersionFile}.");
      }

      // extend here if there are newer DataVersions => Enable backwards compatibility

      var project = new ProjectModel();
      serializer.Populate(jsonObject.CreateReader(), project);
      return project;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      // not supported
      throw new NotImplementedException();
    }
  }
}