using FalloutVault.Devices.Interfaces;

public interface ICropSprinklerController : IOnOff
{
    bool IsOn { get; set; }
    int _TargetSection { get; set; }
    double TargetLitres { get; set; }
    double MinutesOn { get; set; }
}