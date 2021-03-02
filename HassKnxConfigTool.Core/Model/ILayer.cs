namespace HassKnxConfigTool.Core.Model
{
  public interface ILayer
  {
    /// <summary>
    /// Unique Identifier for this layer.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Name of this layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Shows the depth of this layer.
    /// 0 is the top level layer, ...
    /// </summary>
    int Depth { get; }
  }
}