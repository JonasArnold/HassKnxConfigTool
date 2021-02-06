using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Attributes
{
  /// <summary>
  /// Helps with reading out specific attributes assigned to properties.
  /// </summary>
  public class AttributeHelper
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
  }
}
