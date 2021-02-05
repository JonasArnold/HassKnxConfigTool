using Common.Knx;

namespace HassKnxConfigFileGenerator.DeviceTypeDefinitions
{
  /// <summary>
  /// The knx light integration is used as an interface to control KNX actuators for lighting applications such as:
  /// - Switching actuators
  /// - Dimming actuators
  /// - LED controllers 
  /// - DALI gateways
  /// </summary>
  public class Light
  {
    /// <summary>
    /// A name for this device used within Home Assistant.
    /// /// </summary>
    [PropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// KNX group address for switching the light on and off. 
    /// DPT 1.001
    /// </summary>
    [PropertyName("address")]
    public GroupAddress Address { get; set; }

    /// <summary>
    /// KNX group address for retrieving the switch state of the light. 
    /// DPT 1.001
    /// </summary>
    [PropertyName("state_address")]
    public GroupAddress StateAddress { get; set; }

    /// <summary>
    /// KNX group address for setting the brightness of the light in percent (absolute dimming). 
    /// DPT 5.001
    /// </summary>
    [PropertyName("brightness_address")]
    public GroupAddress BrightnessAddress { get; set; }

    /// <summary>
    /// KNX group address for retrieving the brightness of the light in percent. 
    /// DPT 5.001
    /// </summary>
    [PropertyName("brightness_state_address")]
    public GroupAddress BrightnessStateAddress { get; set; }

    // TODO Add Color RGBW, Color Temperature
  }
}
