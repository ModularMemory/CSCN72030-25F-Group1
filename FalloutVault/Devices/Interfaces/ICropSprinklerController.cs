using FalloutVault.Devices.Interfaces;

public interface ICropSprinklerController : IOnOff
{
    int TargetSection { get; }
    int TargetLitres { get;}
    TimeSpan TimeSpanOn { get; }
}