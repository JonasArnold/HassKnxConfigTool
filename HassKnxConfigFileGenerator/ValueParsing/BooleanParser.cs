namespace HassKnxConfigFileGenerator.ValueParsing
{
  internal class BooleanParser : IParser
  {
    public string Parse(object value)
    {
      bool boolean = (bool)value;
      return boolean.ToString().ToLower();
    }
  }
}