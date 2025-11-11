using FalloutVault.Devices.Models;

namespace FalloutVault.Tests.Utils;

public static class DeviceIdGenerator
{
    private static readonly char[] NameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-".ToArray();

    public static DeviceId GetRandomDeviceId(int seed = 0)
    {
        var random = new Random(seed);

        var nameLen = random.Next(3, 5);
        var zoneLen = random.Next(3, 5);

        var name = string.Concat(random.GetItems(NameChars, nameLen));
        var zone = string.Concat(random.GetItems(NameChars, zoneLen));

        return new DeviceId(name, zone);
    }
}