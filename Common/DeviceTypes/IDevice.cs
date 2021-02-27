namespace Common.DeviceTypes
{
  public interface IDevice
  {
    public DeviceTypes Type { get; }
    public string Name { get; set; }
  }
}
