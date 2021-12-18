using Common.Attributes;
using Common.Knx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.DeviceTypes
{
  /// <summary>
  /// The KNX cover platform is used as an interface to KNX covers.
  /// </summary>
  public class Cover : BaseDevice
  {
    /// <summary>
    /// Creates an instance and initializes the name of the device.
    /// </summary>
    /// <param name="name">new name of the device.</param>
    public Cover(string name)
     : this()
    {
      this.Name = name;
    }

    /// <summary>
    /// Creates an instance without initializing a name.
    /// </summary>
    public Cover()
      : base(DeviceType.Cover)
    {
      this.InitEnumValues();
      this.MoveLongAddress = new GroupAddress();
      this.MoveLongAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.MoveShortAddress = new GroupAddress();
      this.MoveShortAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); }; 
      this.StopAddress = new GroupAddress();
      this.StopAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.PositionAddress = new GroupAddress();
      this.PositionAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.PositionStateAddress = new GroupAddress();
      this.PositionStateAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
      this.TravellingTimeUp = 25;   // TODO make configurable
      this.TravellingTimeDown = 25;
      this.InvertedPosition = false;
      this.InvertedAngle = false;
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

    private GroupAddress moveLongAddress;
    /// <summary>
    /// KNX group address for moving the cover full up or down. 
    /// DPT 1
    /// </summary>
    [PropertyName("move_long_address")]
    public GroupAddress MoveLongAddress
    {
      get { return this.moveLongAddress; }
      set { this.moveLongAddress = value; OnPropertyChanged(nameof(this.MoveLongAddress)); }
    }

    private GroupAddress moveShortAddress;
    /// <summary>
    /// KNX group address for moving the cover stepwise up or down. 
    /// Used by some covers also as the means to stop the cover, if no dedicated stop_address exists on the actuator. 
    /// DPT 1
    /// </summary>
    [PropertyName("move_short_address")]
    public GroupAddress MoveShortAddress
    {
      get { return this.moveShortAddress; }
      set { this.moveShortAddress = value; OnPropertyChanged(nameof(this.MoveShortAddress)); }
    }

    private GroupAddress stopAddress;
    /// <summary>
    /// KNX group address for stopping the current movement of the cover.
    /// DPT 1
    /// </summary>
    [PropertyName("stop_address")]
    public GroupAddress StopAddress
    {
      get { return this.stopAddress; }
      set { this.stopAddress = value; OnPropertyChanged(nameof(this.StopAddress)); }
    }

    private GroupAddress positionAddress;
    /// <summary>
    /// KNX group address for moving the cover to the dedicated position. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("position_address")]
    public GroupAddress PositionAddress
    {
      get { return this.positionAddress; }
      set { this.positionAddress = value; OnPropertyChanged(nameof(this.PositionAddress)); }
    }

    private GroupAddress positionStateAddress;
    /// <summary>
    /// Separate KNX group address for requesting the current position of the cover. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("position_state_address")]
    public GroupAddress PositionStateAddress
    {
      get { return this.positionStateAddress; }
      set { this.positionStateAddress = value; OnPropertyChanged(nameof(this.PositionStateAddress)); }
    }

    private bool invertedPosition;
    /// <summary>
    /// Set this to true if your actuator reports fully closed position as 0% in KNX.
    /// </summary>
    [PropertyName("invert_position")]
    public bool InvertedPosition
    {
      get { return this.invertedPosition; }
      set { this.invertedPosition = value; OnPropertyChanged(nameof(this.InvertedPosition)); base.OnAnyPropertyChanged(); }
    }

    private int travellingTimeDown;
    /// <summary>
    /// Time cover needs to travel down in seconds. 
    /// Needed to calculate the intermediate positions of cover while traveling.
    /// </summary>
    [PropertyName("travelling_time_down")]
    public int TravellingTimeDown
    {
      get { return this.travellingTimeDown; }
      set { this.travellingTimeDown = value; OnPropertyChanged(nameof(this.TravellingTimeDown)); }
    }

    private int travellingTimeUp;
    /// <summary>
    /// Time cover needs to travel up in seconds. 
    /// Needed to calculate the intermediate positions of cover while traveling.
    /// </summary>
    [PropertyName("travelling_time_up")]
    public int TravellingTimeUp
    {
      get { return this.travellingTimeUp; }
      set { this.travellingTimeUp = value; OnPropertyChanged(nameof(this.TravellingTimeUp)); }
    }

    /// <summary>
    /// Displays if the Angle is enabled.
    /// If set the instances are created or deleted.
    /// </summary>
    [JsonIgnore]
    public bool AngleEnabled
    {
      get => this.AngleAddress != null;
      set
      {
        if (value)
        {
          this.AngleAddress = new GroupAddress();
          this.AngleAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
          this.AngleStateAddress = new GroupAddress();
          this.AngleStateAddress.PropertyChanged += delegate { base.OnAnyPropertyChanged(); };
        }
        else
        {
          this.AngleAddress = null;
          this.AngleStateAddress = null;
        }
        OnPropertyChanged(nameof(this.AngleEnabled));
      }
    }

    private GroupAddress angleAddress;
    /// <summary>
    /// KNX group address for tilting the cover to the dedicated angle. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("angle_address")]
    public GroupAddress AngleAddress
    {
      get { return this.angleAddress; }
      set { this.angleAddress = value; OnPropertyChanged(nameof(this.AngleAddress)); }
    }

    private GroupAddress angleStateAddress;
    /// <summary>
    /// Separate KNX group address for requesting the current tilt angle of the cover. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("angle_state_address")]
    public GroupAddress AngleStateAddress
    {
      get { return this.angleStateAddress; }
      set { this.angleStateAddress = value; OnPropertyChanged(nameof(this.AngleStateAddress)); }
    }

    private bool invertedAngle;
    /// <summary>
    /// Set this to true if your actuator reports fully closed tilt as 0% in KNX.
    /// </summary>
    [PropertyName("invert_angle")]
    public bool InvertedAngle
    {
      get { return this.invertedAngle; }
      set { this.invertedAngle = value; OnPropertyChanged(nameof(this.InvertedAngle)); base.OnAnyPropertyChanged(); }
    }

    /// <summary>
    /// Provides the values from the enum.
    /// </summary>
    private List<CoverType> _coverTypeValues;
    [JsonIgnore]
    public List<CoverType> CoverTypeValues
    {
      get { return _coverTypeValues; }
      set
      {
        _coverTypeValues = value;
        OnPropertyChanged(nameof(this.CoverTypeValues));
      }
    }

    private CoverType selectedCoverType;
    /// <summary>
    /// Sets the class of the device, changing the device state and icon that is displayed on the frontend.
    /// https://www.home-assistant.io/integrations/cover/
    /// </summary>
    [PropertyName("device_class")]
    public CoverType SelectedCoverType
    {
      get { return this.selectedCoverType; }
      set { this.selectedCoverType = value; OnPropertyChanged(nameof(this.SelectedCoverType)); OnAnyPropertyChanged(); }
    }

    #endregion

    #region Helpers
    /// <summary>
    /// Initializes the Enum <see cref="CoverTypeValues"/>. 
    /// Selects an item for <see cref="CoverType"/>.
    /// </summary>
    private void InitEnumValues()
    {
      // parse enum
      this.CoverTypeValues = Enum.GetValues(typeof(CoverType)).Cast<CoverType>().ToList();

      // select first sensor type
      if (this.CoverTypeValues.Any())
      {
        this.SelectedCoverType = this.CoverTypeValues.First();
      }
    }
    #endregion
  }
}
