using Common.Attributes;
using Common.Knx;

namespace Common.DeviceTypes
{
  /// <summary>
  /// The KNX switch platform is used as an interface to switching actuators.
  /// </summary>
  public class Switch : BaseDevice
  {
    /// <summary>
    /// Creates an instance and initializes the name of the device.
    /// </summary>
    /// <param name="name">new name of the device.</param>
    public Switch(string name)
     : this()
    {
      this.Name = name;
    }

    /// <summary>
    /// Creates an instance without initializing a name.
    /// </summary>
    public Switch()
      : base(DeviceType.Switch)
    {
      this.WireCommands();
      this.Address = new GroupAddress();
      this.Address.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.StateAddress = new GroupAddress();
      this.StateAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.Inverted = false;
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
    /// KNX group address for switching the switch on/off. 
    /// DPT 1
    /// </summary>
    [PropertyName("address")]
    public GroupAddress Address
    {
      get { return this.address; }
      set { this.address = value; OnPropertyChanged(nameof(this.Address)); }
    }

    private GroupAddress stateAddress;
    /// <summary>
    /// Separate KNX group address for retrieving the switch state. 
    /// DPT 1
    /// </summary>
    [PropertyName("state_address")]
    public GroupAddress StateAddress
    {
      get { return this.stateAddress; }
      set { this.stateAddress = value; OnPropertyChanged(nameof(this.StateAddress)); }
    }

    private bool inverted;
    /// <summary>
    /// Invert the telegrams payload before processing or sending.
    /// </summary>
    [PropertyName("invert")]
    public bool Inverted
    {
      get { return this.inverted; }
      set { this.inverted = value; OnPropertyChanged(nameof(this.Inverted)); base.OnAnyPropertyChanged(); }
    }

    #endregion


    #region Commands
    private void WireCommands()
    {
    }

    #endregion
  }
}
