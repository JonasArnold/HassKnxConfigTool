using Common.Mvvm;

namespace Common.Knx
{
  /// <summary>
  /// A KNX Group Address
  /// Example: 1/1/12
  /// </summary>
  public class GroupAddress : ObservableObject
  {
    private uint mainGroup;
    /// <summary>
    /// Main Group (Hauptgruppe)
    /// 0...31
    /// </summary>
    public uint MainGroup
    {
      get { return this.mainGroup; }
      set { this.mainGroup = value; OnPropertyChanged(nameof(this.MainGroup)); }
    }

    private uint middleGroup;
    /// <summary>
    /// Middle Group (Mittelgruppe)
    /// 0...7
    /// </summary>
    public uint MiddleGroup
    {
      get { return this.middleGroup; }
      set { this.middleGroup = value; OnPropertyChanged(nameof(this.MiddleGroup)); }
    }

    private uint subGroup;
    /// <summary>
    /// Sub Group (Untergruppe)
    /// 0...255
    /// </summary>
    public uint SubGroup
    {
      get { return this.subGroup; }
      set { this.subGroup = value; OnPropertyChanged(nameof(this.SubGroup)); }
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
