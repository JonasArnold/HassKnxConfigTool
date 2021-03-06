using Common.DeviceTypes;
using HassKnxConfigTool.Core.Serializing;
using Newtonsoft.Json;
using System;

namespace HassKnxConfigTool.Core.Model
{
  [Serializable]
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  [JsonConverter(typeof(LayerConverter))]
  public class DeviceModel : BaseLayer, IEquatable<DeviceModel>, IEquatable<string>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
  {
    public DeviceModel(ILayer parentLayer)
      :base(parentLayer)
    {
    }

    /// <summary>
    /// Constructor to initialize from deserialization.
    /// </summary>
    public DeviceModel(ILayer parentLayer, int depth, string myId)
  : base(parentLayer, depth, myId)
    {
    }

    [JsonConverter(typeof(DeviceConverter))] // using a specific converter to enable deserialization to instanciate correct implementation of interface
    public IDevice Device { get; set; }

    #region Base class members
    public override bool IsExpanded { get => false; set => throw new NotSupportedException("IsExpanded is not applicable for DeviceModel"); }
    #endregion

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
