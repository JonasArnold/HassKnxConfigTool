using Common.DeviceTypes;
using System;

namespace HassKnxConfigTool.Core.Model
{
  public class DeviceModel : IEquatable<DeviceModel>, IEquatable<string>
  {
    public DeviceModel()
    {
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    public string Name { get; set; }

    public IDevice Device { get; }

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
