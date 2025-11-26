using System.Runtime.CompilerServices;
using FalloutVault.Devices.Models;

namespace FalloutVault.Tests.Utils;

public static class DeviceIdGenerator
{
    private static readonly char[] NameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-".ToArray();

    public static DeviceId GetRandomDeviceId([CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNumber = 0)
        => GetRandomDeviceId(HashCode.Combine(callerName.GetHashCode(), callerLineNumber));

    public static DeviceId GetRandomDeviceId(int seed)
        => new(GetRandomDeviceName(seed), GetRandomDeviceZone(seed));

    public static string GetRandomDeviceName(int seed)
    {
        var random = new Random(seed);

        var nameLen = random.Next(3, 5);

        return string.Concat(random.GetItems(NameChars, nameLen));
    }

    public static string GetRandomDeviceZone(int seed)
    {
        var random = new Random(seed);

        var zoneLen = random.Next(3, 5);

        return string.Concat(random.GetItems(NameChars, zoneLen));
    }
}