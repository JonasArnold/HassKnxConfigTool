using System;

namespace Common.Attributes
{
  /// <summary>
  /// An attribute to define the display name, which the user will see.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
  public class DisplayNameAttribute : Attribute
  {
    /// <summary>
    /// Display name to display to the user.
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// Defines the name that shall be displayed to the user.
    /// </summary>
    public DisplayNameAttribute(string displayName)
    {
      this.DisplayName = displayName;
    }
  }
}
