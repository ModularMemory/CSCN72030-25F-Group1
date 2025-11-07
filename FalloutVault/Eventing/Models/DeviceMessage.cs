namespace FalloutVault.Eventing.Models;

public sealed record DeviceMessage(DateTime Timestamp, object Data);