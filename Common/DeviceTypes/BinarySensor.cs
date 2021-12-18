using Common.Attributes;
using Common.Knx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.DeviceTypes
{
  /// <summary>
  /// The KNX binary sensor platform allows you to monitor KNX binary sensors.
  /// Binary sensors are read-only.
  /// To write to the KNX bus configure an exposure KNX Integration Expose.
  /// </summary>
  public class BinarySensor : BaseDevice
  {
    /// <summary>
    /// Creates an instance and initializes the name of the device.
    /// </summary>
    /// <param name="name">new name of the device.</param>
    public BinarySensor(string name)
     : this()
    {
      this.Name = name;
    }

    /// <summary>
    /// Creates an instance without initializing a name.
    /// </summary>
    public BinarySensor()
      : base(DeviceType.BinarySensor)
    {
      this.WireCommands();
      this.InitEnumValues();
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

    private GroupAddress stateAddress;
    /// <summary>
    /// KNX group address of the binary sensor. 
    /// DPT 1
    /// </summary>
    [PropertyName("state_address")]
    public GroupAddress StateAddress
    {
      get { return this.stateAddress; }
      set { this.stateAddress = value; OnPropertyChanged(nameof(this.StateAddress)); }
    }

    /// <summary>
    /// Provides the values from the enum.
    /// </summary>
    private List<BinarySensorType> _binarySensorTypeValues;
    [JsonIgnore]
    public List<BinarySensorType> BinarySensorTypeValues
    {
      get { return _binarySensorTypeValues; }
      set
      {
        _binarySensorTypeValues = value;
        OnPropertyChanged(nameof(this.BinarySensorTypeValues));
      }
    }

    private BinarySensorType selectedbinarySensorType;
    /// <summary>
    /// Sets the class of the device, changing the device state and icon that is displayed on the frontend.
    /// https://www.home-assistant.io/integrations/binary_sensor/
    /// </summary>
    [PropertyName("device_class")]
    public BinarySensorType SelectedBinarySensorType
    {
      get { return this.selectedbinarySensorType; }
      set { this.selectedbinarySensorType = value; OnPropertyChanged(nameof(this.SelectedBinarySensorType)); OnAnyPropertyChanged(); }
    }

    private bool inverted;
    /// <summary>
    /// Invert the telegrams payload before processing. 
    /// This is applied before context_timeout or reset_after is evaluated.
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

    #region Helpers
    /// <summary>
    /// Initializes the Enum <see cref="BinarySensorTypeValues"/>. 
    /// Selects an item for <see cref="BinarySensorType"/>.
    /// </summary>
    private void InitEnumValues()
    {
      // parse enum
      this.BinarySensorTypeValues = Enum.GetValues(typeof(BinarySensorType)).Cast<BinarySensorType>().ToList();
      
      // select first sensor type
      if (this.BinarySensorTypeValues.Any())
      {
        this.SelectedBinarySensorType = this.BinarySensorTypeValues.First();
      }
    }
    #endregion
  }
}
