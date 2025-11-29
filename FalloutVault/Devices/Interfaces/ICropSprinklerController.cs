using FalloutVault.Devices.Interfaces;

public interface ICropSprinklerController : IOnOff
{
    int TargetSections { get; }
    int TargetLitres { get;}
    TimeSpan TimeSpanOn { get; }
}