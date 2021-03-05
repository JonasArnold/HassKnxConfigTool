using System;

namespace Common.Attributes
{
  /// <summary>
  /// An attribute to define the exact value of the field. 
  /// Used to parse enum fields to the correct string value.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
  public class FieldNameAttribute : Attribute
  {
    /// <summary>
    /// Name of the field to be exported.
    /// </summary>
    public string FieldName { get; private set; }

    /// <summary>
    /// Defines the exact string to export for the field.
    /// </summary>
    public FieldNameAttribute(string fieldName)
    {
      this.FieldName = fieldName;
    }
  }
}
