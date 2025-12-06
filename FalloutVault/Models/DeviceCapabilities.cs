namespace FalloutVault.Models;

[Flags]
public enum DeviceCapabilities : ulong
{
    None = 0,
    OnOff = 1 << 0,
    TemporaryOff = 1 << 1,
    TemporaryOn = 1 << 2,
    Periodic = 1 << 3,
    OpenClose = 1 << 4,
    Lockable = 1 << 5,
}