using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Attributes
{
  /// <summary>
  /// Helps with reading out specific attributes assigned to properties.
  /// </summary>
  public static class AttributeHelper
  {
    /// <summary>
    /// Reads out the attribute's value of a property. 
    /// Example:
    /// string val = AttributeHelper.GetPropertyInfoAttributeValue<Light, string, PropertyNameAttribute, string>(prop => prop.Name, attr => attr.Value);
    /// </summary>
    public static TValue GetPropertyAttributeValue<T, TOut, TAttribute, TValue>(
    Expression<Func<T, TOut>> propertyExpression,
    Func<TAttribute, TValue> valueSelector)
    where TAttribute : Attribute
    {
      var expression = (MemberExpression)propertyExpression.Body;
      var propertyInfo = (PropertyInfo)expression.Member;
      return propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;
    }

    /// <summary>
    /// Reads out the attribute's value of a PropertyInfo object. 
    /// Example:
    /// string val = AttributeHelper.GetPropertyInfoAttributeValue<T, string, PropertyNameAttribute, string>(propertyInfo, attr => attr.Value);
    /// </summary>
    public static TValue GetPropertyInfoAttributeValue<T, TOut, TAttribute, TValue>(
    PropertyInfo propertyInfo,
    Func<TAttribute, TValue> valueSelector)
    where TAttribute : Attribute
    {
      return propertyInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute attr ? valueSelector(attr) : default;
    }

    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
      var type = enumVal.GetType();
      var memInfo = type.GetMember(enumVal.ToString());
      var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
      return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
  }
}
