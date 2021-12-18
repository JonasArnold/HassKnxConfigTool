using Common.Attributes;
using Common.Knx;

namespace Common.DeviceTypes
{
  /// <summary>
  /// The KNX scenes platform allows you to trigger KNX scenes. These entities are write-only.
  /// </summary>
  public class Scene : BaseDevice
  {
    /// <summary>
    /// Creates an instance and initializes the name of the device.
    /// </summary>
    /// <param name="name">new name of the device.</param>
    public Scene(string name)
     : this()
    {
      this.Name = name;
    }

    /// <summary>
    /// Creates an instance without initializing a name.
    /// </summary>
    public Scene()
      : base(DeviceType.Scene)
    {
      this.Address = new GroupAddress();
      this.Address.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.SceneNumber = 1; // default 1
    }

    #region Properties
    private string name;
    /// <summary>
    /// A name for this device used within Home Assistant.
    /// </summary>
    [PropertyName("name")]
    public override string Name
    {
      get { return this.name; }
      set { this.name = Helpers.StringHelpers.NormalizeString(value); OnPropertyChanged(nameof(this.Name)); OnAnyPropertyChanged(); }
    }

    private GroupAddress address;
    /// <summary>
    /// KNX group address for the scene. 
    /// DPT 17.001
    /// </summary>
    [PropertyName("address")]
    public GroupAddress Address
    {
      get { return this.address; }
      set { this.address = value; OnPropertyChanged(nameof(this.Address)); }
    }

    private int sceneNumber;
    /// <summary>
    /// KNX scene number to be activated (range 1..64 ).
    /// </summary>
    [PropertyName("scene_number")]
    public int SceneNumber
    {
      get { return this.sceneNumber; }
      set { this.sceneNumber = value; OnPropertyChanged(nameof(this.SceneNumber)); OnAnyPropertyChanged(); }
    }
    #endregion
  }
}
