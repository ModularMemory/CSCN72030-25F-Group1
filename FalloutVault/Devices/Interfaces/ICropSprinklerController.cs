using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface ICropSprinklerController : IOnOff
{
    Watt sprinklerWattage { get; }
    int TargetSection { get; }
    int TargetLitres { get;}
    TimeSpan TimeSpanOn { get; }
}