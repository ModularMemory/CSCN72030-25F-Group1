using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface ICropSprinklerController : IOnOff
{
    Watt Wattage { get; }
    SprinklerSection TargetSection { get; }
    int TargetLitres { get;}
    TimeSpan TimeOn { get; }
}