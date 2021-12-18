namespace HassKnxConfigTool.Core
{
  internal static class Constants
  {
    /// <summary>
    /// Serialization / Deserialization
    /// </summary>
    internal const string ProjectFilesLocation = @"C:\HassKnxConfigTool\Projects\";
    internal const string ProjectFilesExtension = ".hkctp";
    internal const string ExportLocation = @"C:\HassKnxConfigTool\Exports\";
    internal const string ExportFolderDateTimeFormat = "MM_dd_yyyy_HH_mm_ss";
    internal const string ExportFilesExtension = ".yaml";

    /// <summary>
    /// Indicates the version of data, which means how the layers are structured and 
    /// what properties are used.
    /// For each change of any models this number shall be incremented.
    /// Important for Serialization / Deserialization!
    /// </summary>
    internal const int DataVersion = 2;

    /// <summary>
    /// Maximum layer depth of tree structure.
    /// </summary>
    internal const int MaxLayerDepth = 3;
  }
}
