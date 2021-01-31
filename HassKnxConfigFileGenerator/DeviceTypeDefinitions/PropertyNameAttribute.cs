using System;

namespace HassKnxConfigFileGenerator.DeviceTypeDefinitions
{
  /// <summary>
  /// An attribute to define the name of a property to be used by the system (real name).
  /// This helps to distinguish between C# property name and backend property name.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
  internal class PropertyNameAttribute : Attribute
  {
    /// <summary>
    /// Backend property name.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Defines the name of a Property for the backend.
    /// </summary>
    /// <param name="propertyName"></param>
    public PropertyNameAttribute(string value)
    {
      this.Value = value;
    }
  }
}
