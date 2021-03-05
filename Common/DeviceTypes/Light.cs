using Common.Attributes;
using Common.Knx;
using Common.Mvvm;
using Newtonsoft.Json;

namespace Common.DeviceTypes
{
  /// <summary>
  /// The knx light integration is used as an interface to control KNX actuators for lighting applications such as:
  /// - Switching actuators
  /// - Dimming actuators
  /// - LED controllers 
  /// - DALI gateways
  /// </summary>
  public class Light : BaseDevice
  {
    /// <summary>
    /// Creates an instance and initializes the name of the device.
    /// </summary>
    /// <param name="name">new name of the device.</param>
    public Light(string name)
     :this()
    {
      this.Name = name;
    }

    /// <summary>
    /// Creates an instance without initializing a name.
    /// </summary>
    public Light()
      :base(DeviceType.Light)
    {
      this.WireCommands();
      this.Address = new GroupAddress();
      this.Address.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.StateAddress = new GroupAddress();
      this.StateAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
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
      set { this.name = value; OnPropertyChanged(nameof(this.Name)); OnAnyPropertyChanged(); }
    }

    private GroupAddress address;
    /// <summary>
    /// KNX group address for switching the light on and off. 
    /// DPT 1.001
    /// </summary>
    [PropertyName("address")]
    public GroupAddress Address
    {
      get { return this.address; }
      set { this.address = value; OnPropertyChanged(nameof(this.Address)); }
    }

    private GroupAddress stateAddress;
    /// <summary>
    /// KNX group address for retrieving the switch state of the light. 
    /// DPT 1.001
    /// </summary>
    [PropertyName("state_address")]
    public GroupAddress StateAddress
    {
      get { return this.stateAddress; }
      set { this.stateAddress = value; OnPropertyChanged(nameof(this.StateAddress)); }
    }

    /// <summary>
    /// Displays if the Brightness is enabled.
    /// If set the isntances are created or deleted.
    /// </summary>
    [JsonIgnore]
    public bool BrightnessEnabled { 
      get => this.BrightnessAddress != null;
      set
      {
        if(value)
        {
          this.BrightnessAddress = new GroupAddress();
          this.BrightnessAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
          this.BrightnessStateAddress = new GroupAddress();
          this.BrightnessStateAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
        }
        else
        {
          this.BrightnessAddress = null;
          this.BrightnessStateAddress = null;
        }
        OnPropertyChanged(nameof(this.BrightnessEnabled));
      }
    }

    private GroupAddress brightnessAddress;
    /// <summary>
    /// KNX group address for setting the brightness of the light in percent (absolute dimming). 
    /// DPT 5.001
    /// </summary>
    [PropertyName("brightness_address")]
    public GroupAddress BrightnessAddress
    {
      get { return this.brightnessAddress; }
      set { this.brightnessAddress = value; OnPropertyChanged(nameof(this.BrightnessAddress)); OnPropertyChanged(nameof(this.BrightnessEnabled)); }
    }

    private GroupAddress brightnessStateAddress;
    /// <summary>
    /// KNX group address for retrieving the brightness of the light in percent. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("brightness_state_address")]
    public GroupAddress BrightnessStateAddress
    {
      get { return this.brightnessStateAddress; }
      set { this.brightnessStateAddress = value; OnPropertyChanged(nameof(this.BrightnessStateAddress)); OnPropertyChanged(nameof(this.BrightnessEnabled)); }
    }

    // TODO Add Color RGBW, Color Temperature
    #endregion

    #region Commands
    private void WireCommands()
    {
      PopulateByPatternCommand = new RelayCommand(PopulatePropertiesByPattern);
    }

    [JsonIgnore]
    public RelayCommand PopulateByPatternCommand { get; private set; }
    [JsonIgnore]
    public bool CanPopulateByPattern => this.StateAddress != null;
    private void PopulatePropertiesByPattern()
    {
      uint main = this.Address.MainGroup;
      uint middle = this.Address.MiddleGroup;
      uint sub = this.Address.SubGroup;

      this.StateAddress = new GroupAddress() { MainGroup = main, MiddleGroup = middle, SubGroup = sub + 2 };
      this.BrightnessAddress = new GroupAddress() { MainGroup = main, MiddleGroup = middle, SubGroup = sub + 3 };
      this.BrightnessStateAddress = new GroupAddress() { MainGroup = main, MiddleGroup = middle, SubGroup = sub + 4 };
    }

    #endregion
  }
}
