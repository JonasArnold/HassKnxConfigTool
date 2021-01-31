namespace HassKnxConfigFileGenerator.Knx
{
  /// <summary>
  /// A KNX Group Address
  /// Example: 1/1/12
  /// </summary>
  public class GroupAddress
  {
    /// <summary>
    /// Main Group (Hauptgruppe)
    /// 0...31
    /// </summary>
    public uint MainGroup { get; private set; }

    /// <summary>
    /// Middle Group (Mittelgruppe)
    /// 0...7
    /// </summary>
    public uint MiddleGroup { get; private set; }

    /// <summary>
    /// Sub Group (Untergruppe)
    /// 0...255
    /// </summary>
    public uint SubGroup { get; private set; }

    public GroupAddress(uint mainGroup, uint middleGroup, uint subGroup)
    {
      this.MainGroup = mainGroup;
      this.MiddleGroup = middleGroup;
      this.SubGroup = subGroup;
    }

    /// <summary>
    /// Creates a string out of the Main/Middle/Sub Group Number.
    /// Example: 1/1/12
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return $"{MainGroup}/{MiddleGroup}/{SubGroup}";
    }
  }
}
