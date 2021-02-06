using System;
using System.IO;
using System.Text;

namespace Common.FileHelpers
{
  /// <summary>
  /// File Generation Helper.
  /// </summary>
  public static class FileGenerator
  {
    /// <summary>
    /// Creates a file at the given filePath with the given content.
    /// Will throw exceptions if File.Create does not work, see: <see cref="FileStream"/>.
    /// </summary>
    /// <param name="filePath">Example: @"c:\temp\MyTest.txt"</param>
    /// <param name="content">the content to write to the file</param>
    /// <exception cref="ArgumentException">If there exists already a file with this name.</exception>
    public static void CreateFile(string filePath, string content)
    {
      if (File.Exists(filePath))
      {
        throw new ArgumentException($"File at path does already exist: {filePath}");
      }

      // Create the file, or overwrite if the file exists.
      using FileStream fs = File.Create(filePath);
      // Add some information to the file.
      byte[] byteContent = new UTF8Encoding(true).GetBytes(content);
      fs.Write(byteContent, 0, byteContent.Length);
    }
  }
}
