using Common.DeviceTypes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace HassKnxConfigTool.Core.Model
{
  public class DeviceModel : IEquatable<DeviceModel>, IEquatable<string>
  {
    public DeviceModel()
    {
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    public string Name { get; set; }

    [JsonConverter(typeof(DeviceConverter))] // using a specific converter to enable deserialization to instanciate correct implementation of interface
    public IDevice Device { get; set; }

    /// <summary>
    /// Unique Identifier for this device
    /// </summary>
    public string Id { get; private set; }

    #region IEquatable
    public bool Equals(DeviceModel other)
    {
      return this.Id == other.Id;
    }

    public bool Equals(string id)
    {
      return this.Id == id;
    }
    #endregion
  }
}
