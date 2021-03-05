using System.ComponentModel;

namespace Common.DeviceTypes
{
  /// <summary>
  /// Binary sensor class types from:
  /// https://www.home-assistant.io/integrations/binary_sensor/
  /// </summary>
  public enum BinarySensorType
  {
    [Common.Attributes.DisplayName("None")]
    [Common.Attributes.FieldName("None")]
    [Description("normal binary sensor")]
    None,

    [Common.Attributes.DisplayName("Battery")]
    [Common.Attributes.FieldName("battery")]
    [Description("on = low, off = normal")]
    Battery,

    [Common.Attributes.DisplayName("Battery charging")]
    [Common.Attributes.FieldName("battery_charging")]
    [Description("on = charging, off = not charging")]
    BatteryCharging,

    [Common.Attributes.DisplayName("Cold")]
    [Common.Attributes.FieldName("cold")]
    [Description("on = cold, off = normal")]
    Cold,

    [Common.Attributes.DisplayName("Connectivity")]
    [Common.Attributes.FieldName("connectivity")]
    [Description("on = connected, off = disconnected")]
    Connectivity,

    [Common.Attributes.DisplayName("Door")]
    [Common.Attributes.FieldName("door")]
    [Description("on = open, off = closed")]
    Door,

    [Common.Attributes.DisplayName("Garage door")]
    [Common.Attributes.FieldName("garage_door")]
    [Description("on = open, off = closed")]
    GarageDoor,

    [Common.Attributes.DisplayName("Gas")]
    [Common.Attributes.FieldName("gas")]
    [Description("on = gas detected, off = no gas (clear)")]
    Gas,

    [Common.Attributes.DisplayName("Heat")]
    [Common.Attributes.FieldName("heat")]
    [Description("on = hot, off = normal")]
    Heat,

    [Common.Attributes.DisplayName("Light")]
    [Common.Attributes.FieldName("light")]
    [Description("on = light detected, off = no light")]
    Light,

    [Common.Attributes.DisplayName("Lock")]
    [Common.Attributes.FieldName("lock")]
    [Description("on = open (unlocked), off = closed (locked)")]
    Locked,

    [Common.Attributes.DisplayName("Moisture")]
    [Common.Attributes.FieldName("moisture")]
    [Description("on = moisture (wet), off = no moisture (dry)")]
    Moisture,

    [Common.Attributes.DisplayName("Motion")]
    [Common.Attributes.FieldName("motion")]
    [Description("on = motion detected, off = no motion (clear)")]
    Motion,

    [Common.Attributes.DisplayName("Moving")]
    [Common.Attributes.FieldName("moving")]
    [Description("on = moving, off = not moving (stopped)")]
    Moving,

    [Common.Attributes.DisplayName("Occupancy")]
    [Common.Attributes.FieldName("occupancy")]
    [Description("on = occupied, off = not occupied (clear)")]
    Occupancy,

    [Common.Attributes.DisplayName("Opening")]
    [Common.Attributes.FieldName("opening")]
    [Description("on = open, off = closed")]
    Opening,

    [Common.Attributes.DisplayName("Plug")]
    [Common.Attributes.FieldName("plug")]
    [Description("on = device plugged in, off = device unplugged")]
    Plug,

    [Common.Attributes.DisplayName("Power")]
    [Common.Attributes.FieldName("power")]
    [Description("on = power detected, off = no power")]
    Power,

    [Common.Attributes.DisplayName("Presence")]
    [Common.Attributes.FieldName("presence")]
    [Description("on = home, off = away")]
    Presence,

    [Common.Attributes.DisplayName("Problem")]
    [Common.Attributes.FieldName("problem")]
    [Description("on = problem detected, off = no problem (OK)")]
    Problem,

    [Common.Attributes.DisplayName("Safety")]
    [Common.Attributes.FieldName("safety")]
    [Description("on = unsafe, off = safe")]
    Safety,

    [Common.Attributes.DisplayName("Smoke")]
    [Common.Attributes.FieldName("smoke")]
    [Description("on = smoke detected, off = no smoke (clear)")]
    Smoke,

    [Common.Attributes.DisplayName("Sound")]
    [Common.Attributes.FieldName("sound")]
    [Description("on = sound detected, off = no sound (clear)")]
    Sound,

    [Common.Attributes.DisplayName("Vibration")]
    [Common.Attributes.FieldName("vibration")]
    [Description("on = vibration detected, off = no vibration (clear)")]
    Vibration,

    [Common.Attributes.DisplayName("Window")]
    [Common.Attributes.FieldName("window")]
    [Description("on = open, off = closed")]
    Window
  }
}