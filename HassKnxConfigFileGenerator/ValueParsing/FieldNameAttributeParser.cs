using Common.Attributes;
using Common.Exceptions;

namespace HassKnxConfigFileGenerator.ValueParsing
{
  internal class FieldNameAttributeParser<T> : IParser where T : System.Enum
  {
    public string Parse(object value)
    {
      // get attribute of type
      var fieldNameAttribute = AttributeHelper.GetAttributeOfType<FieldNameAttribute>((T)value);

      if(fieldNameAttribute != null)
      {
        return fieldNameAttribute.FieldName;
      }

      //T item = (T)value;

      //// read all attributes
      //Attribute[] attrs = Attribute.GetCustomAttributes(item.GetType());  // Reflection.  

      //// go through all attributes
      //foreach (Attribute attr in attrs)
      //{
      //  // found correct attribute
      //  if (attr is FieldNameAttribute fieldNameAttribute)
      //  {
      //    return fieldNameAttribute.FiedName; // exit here
      //  }
      //}

      // if not found throw
      throw new ImplementationException($"Value {value} of type {((T)value).GetType()} has no FieldNameAttribute attached.");
    }
  }
}
