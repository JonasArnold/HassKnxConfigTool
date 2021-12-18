namespace HassKnxConfigFileGenerator.ValueParsing
{
  internal class IntegerParser : IParser
  {
    public string Parse(object value)
    {
      return value.ToString();
    }
  }
}