using Common.DeviceTypes;
using Newtonsoft.Json;
using System;

namespace HassKnxConfigTool.Core.Model
{
  [Serializable]
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  public class DeviceModel : BaseLayer, IEquatable<DeviceModel>, IEquatable<string>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  {
    public DeviceModel(ILayer parentLayer)
      :base(parentLayer)
    {
    }

    [JsonConverter(typeof(DeviceConverter))] // using a specific converter to enable deserialization to instanciate correct implementation of interface
    public IDevice Device { get; set; }


    #region IEquatable members
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
