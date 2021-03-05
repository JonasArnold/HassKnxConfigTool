namespace HassKnxConfigFileGenerator
{
  /// <summary>
  /// Interface to implement a parser.
  /// </summary>
  internal interface IParser
  {
    /// <summary>
    /// Parsing method. Parses object to string.
    /// </summary>
    /// <param name="value">object to parse</param>
    /// <returns>parsed string</returns>
    public string Parse(object value);
  }
}