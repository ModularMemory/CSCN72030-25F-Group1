namespace FalloutVault.Devices.Interfaces;

public interface ILightController : ITemporaryOn, ITemporaryOff
{
    bool IsOn { get; set; }
    double DimmerLevel { get; set; }
}