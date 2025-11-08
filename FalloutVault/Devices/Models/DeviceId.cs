namespace FalloutVault.Devices.Models;

public readonly record struct DeviceId
{
    /// <summary>
    /// The name of the device.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The zone where the device is located.
    /// </summary>
    public string Zone { get; }

    public DeviceId(string name, string zone)
    {
        Name = name;
        Zone = zone;
    }

    public override string ToString()
    {
        return $"{Zone} - {Name}";
    }
}