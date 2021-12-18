using System.Globalization;
using System.Text;

namespace Common.Helpers
{
  /// <summary>
  /// Some helper functions to simplify string handling.
  /// </summary>
  public static class StringHelpers
  {
    /// <summary>
    /// Normalizes a string, converts german Umlauts to their corresponding ascii characters,
    /// removes accends etc.
    /// </summary>
    /// <param name="str">string to normalize</param>
    /// <returns>normalized string</returns>
    public static string NormalizeString(string str)
    {
      // Latinize German characters
      StringBuilder sb = new StringBuilder(str.Length);
      foreach (char c in str)
      {
        switch (c)
        {
          case 'ä':
            sb.Append("ae");
            break;
          case 'ö':
            sb.Append("oe");
            break;
          case 'ü':
            sb.Append("ue");
            break;
          case 'Ä':
            sb.Append("Ae");
            break;
          case 'Ö':
            sb.Append("Oe");
            break;
          case 'Ü':
            sb.Append("Ue");
            break;
          case 'ß':
            sb.Append("ss");
            break;
          default:
            sb.Append(c);
            break;
        }
      }
      string latinizedStr = sb.ToString();

      // Remove accents, etc.
      var normalizedString = latinizedStr.Normalize(NormalizationForm.FormD);
      var stringBuilder = new StringBuilder();

      foreach (var c in normalizedString.EnumerateRunes())
      {
        var unicodeCategory = Rune.GetUnicodeCategory(c);
        if (unicodeCategory != UnicodeCategory.NonSpacingMark)
        {
          stringBuilder.Append(c);
        }
      }

      return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

  }
}
