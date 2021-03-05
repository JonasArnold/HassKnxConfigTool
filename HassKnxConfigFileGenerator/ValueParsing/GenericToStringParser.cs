namespace HassKnxConfigFileGenerator.ValueParsing
{
  internal class GenericToStringParser : IParser
  {
    public string Parse(object value)
    {
      return value.ToString();
    }
  }
}
