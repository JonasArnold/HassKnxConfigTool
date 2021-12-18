using System.ComponentModel;

namespace Common.DeviceTypes
{
  /// <summary>
  /// Cover device class types from:
  /// https://www.home-assistant.io/integrations/cover/
  /// </summary>
  public enum CoverType
  {
    [Common.Attributes.DisplayName("None")]
    [Common.Attributes.FieldName("None")]
    [Description("Generic cover.")]
    None,

    [Common.Attributes.DisplayName("Awning")]
    [Common.Attributes.FieldName("awning")]
    [Description("Control of an awning, such as an exterior retractable window, door, or patio cover.")]
    Awning,

    [Common.Attributes.DisplayName("Blind")]
    [Common.Attributes.FieldName("blind")]
    [Description("Control of blinds, which are linked slats that expand or collapse to cover an opening or may be tilted to partially covering an opening, such as window blinds.")]
    Blind,

    [Common.Attributes.DisplayName("Curtain")]
    [Common.Attributes.FieldName("curtain")]
    [Description("Control of curtains or drapes, which is often fabric hung above a window or door that can be drawn open.")]
    Curtain,

    [Common.Attributes.DisplayName("Damper")]
    [Common.Attributes.FieldName("damper")]
    [Description("Control of a mechanical damper that reduces airflow, sound, or light.")]
    Damper,

    [Common.Attributes.DisplayName("Door")]
    [Common.Attributes.FieldName("door")]
    [Description("Control of a door or gate that provides access to an area.")]
    Door,

    [Common.Attributes.DisplayName("Garage")]
    [Common.Attributes.FieldName("garage")]
    [Description("Control of a garage door that provides access to a garage.")]
    Garage,

    [Common.Attributes.DisplayName("Gate")]
    [Common.Attributes.FieldName("gate")]
    [Description("Control of a gate. Gates are found outside of a structure and are typically part of a fence.")]
    Gate,

    [Common.Attributes.DisplayName("Shade")]
    [Common.Attributes.FieldName("shade")]
    [Description("Control of shades, which are a continuous plane of material or connected cells that expanded or collapsed over an opening, such as window shades.")]
    Shade,

    [Common.Attributes.DisplayName("Shutter")]
    [Common.Attributes.FieldName("shutter")]
    [Description("Control of shutters, which are linked slats that swing out/in to covering an opening or may be tilted to partially cover an opening, such as indoor or exterior window shutters.")]
    Shutter,

    [Common.Attributes.DisplayName("Window")]
    [Common.Attributes.FieldName("window")]
    [Description("Control of a physical window that opens and closes or may tilt.")]
    Window
  }
}